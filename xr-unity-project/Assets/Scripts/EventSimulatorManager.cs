using UnityEngine;

public class EventSimulationManager : MonoBehaviour {
    public static EventSimulationManager Instance { get; private set; }

    public EventObject CurrentSelectedObject { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SelectObject(EventObject newSelection) {
        if (CurrentSelectedObject == newSelection)
            return;

        if (CurrentSelectedObject != null)
            CurrentSelectedObject.SetSelected(false);

        CurrentSelectedObject = newSelection;

        if (CurrentSelectedObject != null)
            CurrentSelectedObject.SetSelected(true);

        Debug.Log("Selected object: " + CurrentSelectedObject.name);
    }

    public void ClearSelection(EventObject obj) {
        if (CurrentSelectedObject == obj) {
            CurrentSelectedObject.SetSelected(false);
            CurrentSelectedObject = null;
        }
    }
}
