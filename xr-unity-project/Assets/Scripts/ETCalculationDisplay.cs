using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ETCalculationDisplay : MonoBehaviour {
    [Header("UI References")] public TextMeshProUGUI titleText;
    public TextMeshProUGUI summaryText;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI confidenceLabelText;
    public Slider confidenceSlider;

    [Header("Optional Visual")] public Transform waveformTransform;

    private void Start() {
        if (EventSelectionData.Instance == null) {
            if (summaryText != null)
                summaryText.text = "No event data found.";

            if (warningText != null)
                warningText.text = "The simulation could not load event parameters.";

            return;
        }

        var data = EventSelectionData.Instance;

        float baseMultiplier = GetBaseEventMultiplier(data.eventName);
        float directionFactor = GetDirectionFactor(data.directionQuality);
        float signalStrengthNormalized = data.signalStrength / 100f;

        float distanceFactor = 1f / (1f + (data.distanceKm / 200000f));
        float noiseFactor = 1f + (data.noiseLevel / 50f);

        float effectiveSignal = baseMultiplier * signalStrengthNormalized * distanceFactor * directionFactor;
        float snr = effectiveSignal / noiseFactor;
        float confidence = Mathf.Clamp01(snr * 2.2f);

        float responseEfficiency = directionFactor * 100f;
        float noiseContamination = Mathf.Clamp01((noiseFactor - 1f) / 2f) * 100f;

        string status = GetDetectionStatus(confidence);
        string observationQuality = GetObservationQuality(confidence, noiseContamination);
        string warning = GetWarningText(data.noiseLevel, data.directionQuality, confidence);

        if (titleText != null)
            titleText.text = "Einstein Telescope Response";

        if (summaryText != null) {
            summaryText.text =
                "Simulated Event: " + data.eventName + "\n\n" +
                "Effective Signal: " + effectiveSignal.ToString("F3") + "\n" +
                "Estimated SNR: " + snr.ToString("F3") + "\n" +
                "Detection Confidence: " + (confidence * 100f).ToString("F0") + "%\n" +
                "Noise Contamination: " + noiseContamination.ToString("F0") + "%\n" +
                "Response Efficiency: " + responseEfficiency.ToString("F0") + "%\n" +
                "Observation Quality: " + observationQuality + "\n" +
                "Status: " + status;
        }

        if (warningText != null)
            warningText.text = warning;

        if (confidenceSlider != null)
            confidenceSlider.value = confidence;

        if (confidenceLabelText != null)
            confidenceLabelText.text = "Detection Confidence: " + (confidence * 100f).ToString("F0") + "%";

        if (waveformTransform != null) {
            float yScale = 0.5f + effectiveSignal * 4f;
            waveformTransform.localScale = new Vector3(
                waveformTransform.localScale.x,
                yScale,
                waveformTransform.localScale.z
            );
        }
    }

    private float GetBaseEventMultiplier(string eventName) {
        switch (eventName) {
            case "Black Hole Merger":
                return 1.0f;
            case "Neutron Star Merger":
                return 0.85f;
            case "Burst Event":
                return 0.65f;
            default:
                return 0.75f;
        }
    }

    private float GetDirectionFactor(string directionQuality) {
        switch (directionQuality) {
            case "Favorable":
                return 1.0f;
            case "Medium":
                return 0.75f;
            case "Poor":
                return 0.5f;
            default:
                return 0.75f;
        }
    }

    private string GetDetectionStatus(float confidence) {
        if (confidence > 0.75f)
            return "Strong Detection";
        if (confidence > 0.45f)
            return "Probable Detection";
        if (confidence > 0.2f)
            return "Weak / Uncertain Detection";
        return "No Clear Detection";
    }

    private string GetObservationQuality(float confidence, float noiseContamination) {
        if (confidence > 0.8f && noiseContamination < 30f)
            return "Excellent";
        if (confidence > 0.55f && noiseContamination < 50f)
            return "Good";
        if (confidence > 0.25f)
            return "Limited";
        return "Poor";
    }

    private string GetWarningText(float noiseLevel, string directionQuality, float confidence) {
        string warning = "";

        if (noiseLevel > 70f)
            warning += "Warning: High terrestrial noise detected.\n";

        if (directionQuality == "Poor")
            warning += "Warning: Unfavorable detector geometry.\n";

        if (confidence < 0.2f)
            warning += "Result: Signal falls below confident detection range.\n";

        if (warning == "")
            warning = "Detector conditions suitable for observation.";

        return warning.Trim();
    }
}
