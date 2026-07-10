using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWorld", menuName = "Data/WorldData")]
public class WorldData : ScriptableObject
{
    public string worldName;
    public Sprite backgroundSprite; // 월드 배경 이미지 ( 시간 남으면 )

    [Header("월드안에 스테이지")]
    public List<StageData> stages; // 월드소속 스테이지
}
