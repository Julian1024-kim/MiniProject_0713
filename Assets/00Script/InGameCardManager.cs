using UnityEngine;
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
     void Start()
    {
       if (PlayerInventory.instance.selectedPlantIds.Count >0)
        {
            SetupInGameCards();
        }
    }

    public void SetupInGameCards()
    {
        Debug.Log("인게임 카드 생성 시작!");

        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (int id in PlayerInventory.instance.selectedPlantIds)
        {
            Debug.Log($"카드 생성 중: ID {id}");
            GameObject go = Instantiate(plantCardPrefab, cardContainer);
            PlantCard card = go.GetComponent<PlantCard>();

            card.InitCard(id);
        }
    }
}
