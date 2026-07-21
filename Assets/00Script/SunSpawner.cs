using System.Collections;
using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    public GameObject sunPrefab;
    public float spawnInterval = 5f;
    public float xRange = 8f;
    public float ySpawnPos = 6f;

    private bool isNight = false;

    void Start()
    {
        StartCoroutine(SpawnSunRoutine());
    }

    public IEnumerator SpawnSunRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            if (!isNight)
            {
                SpawnSun();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public void SetNightMode(bool night)
    {
        isNight = night;
    }

    void SpawnSun()
    {
        float randomX = Random.Range(-xRange, xRange);
        float ramdomSpawnY = Random.Range(-2f, 4f);
        Vector3 spawnPos = new Vector3(randomX, ySpawnPos, 0);

       GameObject sunObj = ObjectPoolManager.instance.SpawnFromPool("Sun", spawnPos, Quaternion.identity);
        sunObj.GetComponent<Sun>().Initialize(transform.position.y, true);
    }
}