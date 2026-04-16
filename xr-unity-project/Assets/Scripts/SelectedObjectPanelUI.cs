using TMPro;
using UnityEngine;

public class SelectedObjectPanelUI : MonoBehaviour
{
    public Transform earthTransform;
    public GameObject contentRoot;

    public TextMeshProUGUI selectedObjectText;
    public TextMeshProUGUI simulatedEventText;
    public TextMeshProUGUI distanceText;

    [Header("Offsets")]
    public float heightOffset = 0.3f;
    public float cameraOffset = 0.5f;

    private void Update()
    {
        if (EventSimulationManager.Instance == null)
            return;

        EventObject current = EventSimulationManager.Instance.CurrentSelectedObject;

        if (current == null)
        {
            if (contentRoot != null)
                contentRoot.SetActive(false);
            return;
        }

        if (contentRoot != null)
            contentRoot.SetActive(true);

        if (Camera.main != null)
        {
            Vector3 toCamera = (Camera.main.transform.position - current.transform.position).normalized;

            transform.position =
                current.transform.position +
                Vector3.up * heightOffset +
                toCamera * cameraOffset;

            transform.forward = (transform.position - Camera.main.transform.position).normalized;
        }

        float unityDistance = 0f;
        float displayDistanceKm = 0f;

        if (earthTransform != null)
        {
            unityDistance = Vector3.Distance(current.transform.position, earthTransform.position);
            displayDistanceKm = current.GetDisplayDistanceKm(unityDistance);
        }

        if (selectedObjectText != null)
            selectedObjectText.text = "Selected: " + current.GetDisplayName();

        if (simulatedEventText != null)
            simulatedEventText.text = "Simulated Event: " + current.GetSimulatedEventName();

        if (distanceText != null)
            distanceText.text = "Distance from Earth: " + displayDistanceKm.ToString("N0") + " km";
    }
}
