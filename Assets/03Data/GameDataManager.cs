using UnityEngine;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    private Dictionary<int, PlantInfo> plants = new Dictionary<int, PlantInfo>();
    private Dictionary<int, ZombieInfo> zombies = new Dictionary<int, ZombieInfo>();

    [SerializeField] private PlantData plantData;
    [SerializeField] private ZombieData zombieData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 풀링된 오브젝트가 OnEnable에서 바로 데이터를 조회할 수 있도록
            // Start()가 아닌 Awake()에서 미리 로드해둔다.
            LoadPlantData();
            LoadZombieData();
        }
        else
        {
            Destroy(gameObject); // 중복 생성된 오브젝트만 파괴
            return;
        }
    }

    private void LoadPlantData()
    {
        plants.Clear();
        for (int i = 0; i < plantData.plantList.Count; i++)
        {
            PlantInfo info = plantData.plantList[i].Clone(); // 원본 자체 복사 방지
            plants[info.id] = info;
        }
    }

    private void LoadZombieData()
    {
        zombies.Clear();
        for (int i = 0; i < zombieData.zombieList.Count; i++)
        {
            ZombieInfo info = zombieData.zombieList[i].Clone();
            zombies[info.id] = info;
        }
    }

    public PlantInfo GetPlantInfo(int id)
    {
        return plants.GetValueOrDefault(id);
    }

    public ZombieInfo GetZombieInfo(int id)
    {
        return zombies.GetValueOrDefault(id);
    }
}