using TMPro;
using UnityEngine;

public class ETSceneReceiver : MonoBehaviour {
    public TextMeshProUGUI outputText;

    private void Start() {
        if (EventSelectionData.Instance == null) {
            outputText.text = "No event data found.";
            Debug.LogError("No EventSelectionData instance found.");
            return;
        }

        var data = EventSelectionData.Instance;

        outputText.text =
            "Loaded Event Parameters\n\n" +
            "Selected Source: " + data.objectName + "\n" +
            "Simulated Event: " + data.eventName + "\n" +
            "Distance from Earth: " + data.distanceKm.ToString("N0") + " km\n" +
            "Signal Strength: " + data.signalStrength.ToString("F0") + "\n" +
            "Noise Level: " + data.noiseLevel.ToString("F0") + "\n" +
            "Direction Quality: " + data.directionQuality;

        Debug.Log("ET Scene loaded data successfully.");
    }
}
