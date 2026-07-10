using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    public string name;
    public GameObject prefab;
    public int size;
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    public List<PoolItem> itemsToPool;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolItem item in itemsToPool)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            //부모 오브젝트
            GameObject container = new GameObject(item.name + "_Pool");
            container.transform.SetParent(this.transform);

            for (int i = 0; i < item.size; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);

                obj.transform.SetParent(container.transform);

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(item.name, objectPool);
        }
    }
    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(name)) return null;

        GameObject objectToSpawn = poolDictionary[name].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
