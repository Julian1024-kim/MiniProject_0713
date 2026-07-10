using UnityEngine;
using System.Collections.Generic;

public class InGameCardManager : MonoBehaviour
{
    public static InGameCardManager instance;

    [Header("설정")]
    [SerializeField] GameObject plantCardPrefab; 
    [SerializeField] Transform cardContainer;   

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // 준비 패널에서 '시작' 버튼을 누르면 호출될 함수
    public void SetupInGameCards()
    {
        Debug.Log("인게임 카드 생성 시작!");
        // 1. 기존에 있던 카드들 청소
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. PlayerInventory에서 선택된 ID들만 가져와서 생성
        foreach (int id in PlayerInventory.instance.selectedPlantIds)
        {
            Debug.Log($"카드 생성 중: ID {id}");
            GameObject go = Instantiate(plantCardPrefab, cardContainer);
            PlantCard card = go.GetComponent<PlantCard>();

            // 이 부분을 추가해서 카드가 스스로의 정보를 갱신하게 합니다.
            card.InitCard(id);
        }
    }
}
