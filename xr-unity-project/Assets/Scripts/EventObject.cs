using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EventObject : MonoBehaviour
{
    public enum EventVisualType
    {
        RedStar,
        GreenStar,
        BlueStar
    }

    [Header("Basic Info")]
    public EventVisualType visualType;

    [Header("Distance Mapping")]
    public float maxSceneDistance = 50f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isSelected;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (EventSimulationManager.Instance != null)
        {
            EventSimulationManager.Instance.SelectObject(this);
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (EventSimulationManager.Instance != null &&
            EventSimulationManager.Instance.CurrentSelectedObject == this &&
            ParameterPanelUI.Instance != null)
        {
            ParameterPanelUI.Instance.ShowForObject(this);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        Debug.Log(gameObject.name + " selected = " + isSelected);
    }

    public string GetDisplayName()
    {
        switch (visualType)
        {
            case EventVisualType.RedStar:
                return "Red Star";
            case EventVisualType.GreenStar:
                return "Green Star";
            case EventVisualType.BlueStar:
                return "Blue Star";
            default:
                return "Unknown";
        }
    }

    public string GetSimulatedEventName()
    {
        switch (visualType)
        {
            case EventVisualType.RedStar:
                return "Black Hole Merger";
            case EventVisualType.GreenStar:
                return "Burst Event";
            case EventVisualType.BlueStar:
                return "Neutron Star Merger";
            default:
                return "Unknown Event";
        }
    }

    public float GetDisplayDistanceKm(float unityDistance)
    {
        float normalized = Mathf.Clamp01(unityDistance / maxSceneDistance);

        switch (visualType)
        {
            case EventVisualType.RedStar:
                return Mathf.Lerp(50000f, 500000f, normalized);

            case EventVisualType.GreenStar:
                return Mathf.Lerp(10000f, 150000f, normalized);

            case EventVisualType.BlueStar:
                return Mathf.Lerp(20000f, 300000f, normalized);

            default:
                return Mathf.Lerp(10000f, 100000f, normalized);
        }
    }
}
