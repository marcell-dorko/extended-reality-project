using UnityEngine;

public class AsteroidFloat : MonoBehaviour
{
    [Header("Drift")]
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;

    [Header("Tumble")]
    public float minTumble = 5f;
    public float maxTumble = 30f;

    private Vector3 driftDirection;
    private float driftSpeed;

    void Start()
    {
        driftDirection = Random.onUnitSphere;
        driftSpeed = Random.Range(minSpeed, maxSpeed);

        Vector3 randomTumble = new Vector3(
            Random.Range(minTumble, maxTumble),
            Random.Range(minTumble, maxTumble),
            Random.Range(minTumble, maxTumble)
        );

        GetComponent<Rigidbody>().angularVelocity = randomTumble * Mathf.Deg2Rad;
    }

    void Update()
    {
        transform.Translate(driftDirection * driftSpeed * Time.deltaTime, Space.World);
    }
}