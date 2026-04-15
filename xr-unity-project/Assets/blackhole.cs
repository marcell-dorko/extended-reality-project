using UnityEngine;

public class BlackHoleBirth : MonoBehaviour
{
    [Header("Spawn Area")]
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(40f, 20f, 40f);

    [Header("Birth Animation")]
    public float growDuration = 2.5f;
    public Vector3 fullSize = new Vector3(3f, 3f, 3f);

    [Header("Repeat")]
    public bool repeatSpawn = true;
    public float minTimeBetweenSpawns = 10f;
    public float maxTimeBetweenSpawns = 30f;

    private Vector3 targetScale;
    private bool isGrowing = false;
    private float growTimer = 0f;

    void Start()
    {
        transform.localScale = Vector3.zero;
        targetScale = fullSize;

        
        Spawn();
    }

    void Update()
    {
        if (isGrowing)
        {
            growTimer += Time.deltaTime;
            float progress = growTimer / growDuration;

            // Smooth ease-in curve so it starts slow then expands
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, Mathf.SmoothStep(0f, 1f, progress));

            if (progress >= 1f)
            {
                isGrowing = false;

                if (repeatSpawn)
                {
                    float nextSpawn = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
                    Invoke(nameof(Spawn), nextSpawn);
                }
            }
        }
    }

    void Spawn()
    {
        
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f) + spawnAreaCenter.x;
        float y = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f) + spawnAreaCenter.y;
        float z = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f) + spawnAreaCenter.z;

        transform.position = new Vector3(x, y, z);

        
        transform.localScale = Vector3.zero;
        growTimer = 0f;
        isGrowing = true;
    }
}