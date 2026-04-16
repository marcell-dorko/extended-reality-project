using UnityEngine;

public class EventSelectionData : MonoBehaviour {
    public static EventSelectionData Instance { get; private set; }

    [Header("Saved Event Data")] public string objectName;
    public string eventName;
    public float distanceKm;
    public float signalStrength;
    public float noiseLevel;
    public string directionQuality;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Save(
        string selectedObjectName,
        string selectedEventName,
        float selectedDistanceKm,
        float selectedSignalStrength,
        float selectedNoiseLevel,
        string selectedDirectionQuality) {
        objectName = selectedObjectName;
        eventName = selectedEventName;
        distanceKm = selectedDistanceKm;
        signalStrength = selectedSignalStrength;
        noiseLevel = selectedNoiseLevel;
        directionQuality = selectedDirectionQuality;

        Debug.Log("Saved event data:");
        Debug.Log("Object: " + objectName);
        Debug.Log("Event: " + eventName);
        Debug.Log("Distance: " + distanceKm);
        Debug.Log("Signal Strength: " + signalStrength);
        Debug.Log("Noise Level: " + noiseLevel);
        Debug.Log("Direction Quality: " + directionQuality);
    }
}
