using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public float spawnInterval = 2f; 

    public float minYOffset = -1.5f; 
    public float maxYOffset = 2.0f;

    private float timer;

    public int initialLasersToSpawn = 4;
    public float distanceBetweenLasers = 8f;
    public Vector3 spawnDirection = Vector3.back;

    void Start()
    {
        SpawnInitialLasers();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnLaser(transform.position);
            timer = 0f; 
        }
    }

    void SpawnInitialLasers()
    {
        for (int i = 0; i < initialLasersToSpawn; i++)
        {
            Vector3 corridorOffset = spawnDirection.normalized * (distanceBetweenLasers * (i + 1));
            Vector3 startPos = transform.position + corridorOffset;
            
            SpawnLaser(startPos);
        }
    }
    void SpawnLaser(Vector3 basePosition)
    {
        float randomHeight = Random.Range(minYOffset, maxYOffset);
        
        Vector3 finalPosition = basePosition + (transform.up * randomHeight);
                               
        Instantiate(laserPrefab, finalPosition, laserPrefab.transform.rotation);
    }
}