using UnityEngine;

public class PreparationManager : MonoBehaviour
{
    public static PreparationManager instance;

    [SerializeField] GameObject prepCardPrefab;
    [SerializeField] Transform contentParent;

    void Awake() => instance = this;

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

        InGameCardManager.instance.SetupInGameCards();
        gameObject.SetActive(false);
    }

    public void UpdateAllCards()
    {
        PrepCard[] cards = GetComponentsInChildren<PrepCard>();
        foreach (var card in cards)
        {
            card.RefreshUI();
        }
    }
}
