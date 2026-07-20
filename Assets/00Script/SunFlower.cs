using UnityEngine;

public class SunFlower : Plant
{
    [Header("태양설정")]
    [SerializeField] GameObject sunPrefab;
    [SerializeField] float produceInterval = 24;
    [SerializeField] Transform spawnPoint;

    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= produceInterval)
        {
            ProduceSun();
            timer = 0;
        }
    }

    void ProduceSun()
    {
       GameObject sunObj = ObjectPoolManager.instance.SpawnFromPool("Sun", transform.position, Quaternion.identity);

        sunObj.GetComponent<Sun>().Initialize(transform.position.y, false);
    }
}
