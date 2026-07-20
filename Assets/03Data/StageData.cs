using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Wave
{
    public string waveName;
    public List<GameObject> zombiePrefabs; // [0]은 일반, [1]은 강화 좀비
    public int count; // 소환할  좀비 총 마릿수
    public float spawnInterval;
    public bool transitionToNight;

    [Tooltip("몇 마리마다 강화 좀비를 소환할지 설정 (0이면 소환 안 함)")]
    public int strongZombieInterval;
}

[CreateAssetMenu(fileName = "NewStage", menuName = "Data/StageData")]
public class StageData : ScriptableObject
{
    public string stageName;
    public List<Wave> waves;

    public int stageIndex;
    public int ClearCoin = 100;
}