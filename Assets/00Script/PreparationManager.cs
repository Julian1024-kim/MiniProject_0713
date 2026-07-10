using UnityEngine;
using System.Collections.Generic;

public class PreparationManager : MonoBehaviour
{
    public static PreparationManager instance;

    [SerializeField] GameObject prepCardPrefab; // 준비용 카드 프리팹
    [SerializeField] Transform contentParent;  // Scroll View의 Content

    void Awake() => instance = this;

    // 패널이 켜질 때마다 리스트를 새로고침
    void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        // 기존 카드 삭제
        foreach (Transform child in contentParent) Destroy(child.gameObject);

        // 상점에서 산 식물들 생성
        foreach (int id in PlayerInventory.instance.ownedPlantIds)
        {
            GameObject go = Instantiate(prepCardPrefab, contentParent);
            PrepCard card = go.GetComponent<PrepCard>();
            card.Setup(id);
        }
    }

    // 시작 버튼을 눌렀을 때 실행될 함수
    public void OnClickStartBattle()
    {
        Debug.Log("전투 시작 버튼 클릭됨!");

        if (PlayerInventory.instance.selectedPlantIds.Count == 0)
        {
            Debug.Log("선택된 식물이 없습니다!");
            return;
        }

        // 하단 인게임 카드 생성 요청
        InGameCardManager.instance.SetupInGameCards();

        // 준비 패널 끄기
        gameObject.SetActive(false);
    }

    // 카드가 클릭됐을 때 모든 카드의 체크표시를 새로고침하기 위한 함수
    public void UpdateAllCards()
    {
        PrepCard[] cards = GetComponentsInChildren<PrepCard>();
        foreach (var card in cards)
        {
            card.RefreshUI();
        }
    }
}
