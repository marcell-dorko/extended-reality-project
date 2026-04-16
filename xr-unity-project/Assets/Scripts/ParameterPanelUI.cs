using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParameterPanelUI : MonoBehaviour {
    public static ParameterPanelUI Instance { get; private set; }

    [Header("Scene References")] public Transform earthTransform;
    public string nextSceneName = "EinsteinTelescopeScene";

    [Header("UI Root")] public GameObject contentRoot;

    [Header("Value Labels")] public TextMeshProUGUI signalStrengthValueText;
    public TextMeshProUGUI noiseLevelValueText;
    public TextMeshProUGUI directionQualityValueText;

    [Header("Controls")] public Slider signalStrengthSlider;
    public Slider noiseLevelSlider;
    public TMP_Dropdown directionDropdown;
    public Button continueButton;

    [Header("Follow Settings")] public float heightOffset = 0.35f;
    public float cameraOffset = 0.55f;

    private EventObject currentObject;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        if (contentRoot != null)
            contentRoot.SetActive(false);

        if (signalStrengthSlider != null)
            signalStrengthSlider.onValueChanged.AddListener(OnSignalStrengthChanged);

        if (noiseLevelSlider != null)
            noiseLevelSlider.onValueChanged.AddListener(OnNoiseLevelChanged);

        if (directionDropdown != null)
            directionDropdown.onValueChanged.AddListener(OnDirectionChanged);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);

        RefreshLabels();
    }

    private void Update() {
        if (currentObject == null)
            return;

        if (Camera.main != null) {
            Vector3 toCamera = (Camera.main.transform.position - currentObject.transform.position).normalized;

            transform.position =
                currentObject.transform.position +
                Vector3.up * heightOffset +
                toCamera * cameraOffset;

            transform.forward = (transform.position - Camera.main.transform.position).normalized;
        }
    }

    public void ShowForObject(EventObject obj) {
        currentObject = obj;

        if (contentRoot != null)
            contentRoot.SetActive(true);

        RefreshLabels();
    }

    public void HidePanel() {
        currentObject = null;

        if (contentRoot != null)
            contentRoot.SetActive(false);
    }

    private void RefreshLabels() {
        if (signalStrengthValueText != null && signalStrengthSlider != null)
            signalStrengthValueText.text = "Signal Strength: " + signalStrengthSlider.value.ToString("F0");

        if (noiseLevelValueText != null && noiseLevelSlider != null)
            noiseLevelValueText.text = "Noise Level: " + noiseLevelSlider.value.ToString("F0");

        if (directionQualityValueText != null && directionDropdown != null)
            directionQualityValueText.text = "Direction Quality: " + directionDropdown.options[directionDropdown.value].text;
    }

    private void OnSignalStrengthChanged(float value) {
        RefreshLabels();
    }

    private void OnNoiseLevelChanged(float value) {
        RefreshLabels();
    }

    private void OnDirectionChanged(int value) {
        RefreshLabels();
    }

    private void OnContinueClicked() {
        if (currentObject == null) {
            Debug.LogWarning("No current object selected.");
            return;
        }

        if (EventSelectionData.Instance == null) {
            Debug.LogError("EventSelectionData instance not found in scene.");
            return;
        }

        float unityDistance = Vector3.Distance(currentObject.transform.position, earthTransform.position);
        float distanceKm = currentObject.GetDisplayDistanceKm(unityDistance);

        string directionValue = "Medium";
        if (directionDropdown != null)
            directionValue = directionDropdown.options[directionDropdown.value].text;

        EventSelectionData.Instance.Save(
            currentObject.GetDisplayName(),
            currentObject.GetSimulatedEventName(),
            distanceKm,
            signalStrengthSlider != null ? signalStrengthSlider.value : 50f,
            noiseLevelSlider != null ? noiseLevelSlider.value : 20f,
            directionValue
        );

        Debug.Log("Loading scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}
