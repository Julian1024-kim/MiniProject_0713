using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Header("월드셋팅")]
    public WorldData currentWorld;
    public int currentStageIndex = 0;

    [Header("보상셋팅")]
    public int clearCoin = 100;
    public int bonusCoinPerStage = 50;


    [HideInInspector]
    public StageData stageData;

    public SunSpawner sunSpawner;

    [Header("비주얼셋팅")]
    public Tilemap[] gridTilemaps;
    public Color nightColor = new Color(0.5f, 0.5f, 0.6f, 1f);
    public float transitionDuration = 3f;

    [Header("스폰셋팅")]
    public float spawnX = 16.5f;
    public float[] lanesY;

    [Header("게임스테이트")]
    public bool isGameOver = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }  
    //void Start()
    //
    //{
    //    if (currentWorld != null && currentWorld.stages.Count > 0)
    //    {
    //        StartStage(currentStageIndex);
    //    }
    //    else
    //    {
    //        Debug.LogError("WorldData가 할당되지 않았거나 스테이지가 비어있습니다!");
    //    }
    //}
    public void LoadAndStartStage(WorldData world,int stageIndex)
    {
        currentWorld = world;
        currentStageIndex = stageIndex;

        StartStage(stageIndex);
    }
    public void StartStage(int index)
    {
        if (index < currentWorld.stages.Count)
        {
            stageData = currentWorld.stages[index];
            currentStageIndex = index;
            isGameOver = false;

            Time.timeScale = 1;

            Debug.Log($"<color=cyan>--- {currentWorld.worldName} : 스테이지 {index + 1} 시작! ---</color>");
            StartCoroutine(PlayStageRoutine());
        }
        else
        {
            Debug.Log("모든 스테이지 클리어!");
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("좀비가 선을 넘었습니다.");

        StopAllCoroutines();
        Time.timeScale = 0;

        if (UIManager.instance != null)
            UIManager.instance.OpenGameOverPanel();
    }

    IEnumerator PlayStageRoutine()
    {
        yield return new WaitForSeconds(2f);

        foreach (var wave in stageData.waves)
        {
            if (isGameOver) yield break;

            if (wave.transitionToNight) ChangeToNight();
            else ChangeToDay();

            Debug.Log($"{wave.waveName} 시작!");

            for (int i = 0; i < wave.count; i++)
            {
                if (isGameOver) yield break;

                int currentCount = i + 1;
                SpawnZombieFromWave(wave, false);

                if (wave.strongZombieInterval > 0 && currentCount % wave.strongZombieInterval == 0)
                {
                    Debug.Log($"{currentCount}마리째 : 강력좀비 소환");
                    SpawnZombieFromWave(wave, true);
                }

                yield return new WaitForSeconds(wave.spawnInterval);
            }

            yield return new WaitUntil(() => isGameOver || GameObject.FindGameObjectsWithTag("Zombie").Length == 0);

            if (isGameOver) yield break;
            yield return new WaitForSeconds(2f);
        }

        if (!isGameOver)
        {
            Debug.Log($"스테이지 {currentStageIndex + 1} 클리어");
            yield return new WaitForSeconds(1.5f);

            OpenClearResult();
        }
    }
    void OpenClearResult()
    {
        int reward = clearCoin + (currentStageIndex * bonusCoinPerStage);

        if (CoinManager.instance != null)
        {
            CoinManager.instance.AddCoins(reward);
        }
        if(UIManager.instance != null)
        {
            UIManager.instance.OpenClearPanel(reward);
        }
        Time.timeScale = 0;
    }
    public void OnClickNextStageButton()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.CloseAllPanels();
        }
        if (PlayerInventory.instance.selectedPlantIds.Count == 0)
        {
            Debug.Log("선택된 식물이 없습니다!");
            return;
        }
        InGameCardManager.instance.SetupInGameCards();

        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");
        foreach ( GameObject plant in plants)
        {
            plant.SetActive(false);
        }
        StartStage(currentStageIndex + 1);
    }

    void ChangeToNight()
    {
        SetStageVisual(nightColor);
        if (sunSpawner != null) sunSpawner.SetNightMode(true);
    }

    void ChangeToDay()
    {
        SetStageVisual(Color.white);
        if (sunSpawner != null) sunSpawner.SetNightMode(false);
    }

    void SetStageVisual(Color targetColor)
    {
        foreach (Tilemap map in gridTilemaps)
        {
            if (map != null)
            {
                DOTween.To(() => map.color, x => map.color = x, targetColor, transitionDuration)
                       .SetEase(Ease.InOutQuad); // 두트윈 가속설정
            }
        }
    }

    void SpawnZombieFromWave(Wave wave, bool isStrong)
    {
        if (isGameOver) return;
        if (wave.zombiePrefabs == null || wave.zombiePrefabs.Count == 0) return;

        GameObject selectedZombie;
        if (isStrong && wave.zombiePrefabs.Count > 1)
            selectedZombie = wave.zombiePrefabs[1];
        else
            selectedZombie = wave.zombiePrefabs[0];

        float randomY = lanesY[Random.Range(0, lanesY.Length)];
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0);

        if (ObjectPoolManager.instance != null)
            ObjectPoolManager.instance.SpawnFromPool(selectedZombie.name, spawnPos, Quaternion.identity);
    }
}

