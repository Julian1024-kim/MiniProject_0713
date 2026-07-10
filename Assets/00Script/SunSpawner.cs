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

    IEnumerator SpawnSunRoutine()
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

    void SpawnSun()
    {
        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPos = new Vector3(randomX, ySpawnPos, 0);

        ObjectPoolManager.instance.SpawnFromPool("Sun", spawnPos, Quaternion.identity);
        Debug.Log("ÇÞºû »ý¼ºµÊ!");
    }

    public void SetNightMode(bool night)
    {
        isNight = night;
    }
}









