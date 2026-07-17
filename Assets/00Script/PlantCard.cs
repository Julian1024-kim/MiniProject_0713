using UnityEngine;
using TMPro;

public class PlantCard : MonoBehaviour
{
    [Header("이 카드가 생성할 식물 정보")]
    public int plantId;
    public GameObject plantPrefab; // 인스펙터에서 직접 연결할 식물 프리팹

    [SerializeField] TextMeshProUGUI nameText;

    public void OnClickCard()
    {
        PlantInfo info = GameDataManager.instance.GetPlantInfo(plantId);

        if (info != null)
        {
            PlacementManager.instance.SetSelectedPlant(info, plantPrefab);
            Debug.Log($"{info.name} 선택됨! 가격: {info.sunCost}");
        }
    }

    // 카드가 생성될 때 이름만 셋팅
    public void InitCard(int id)
    {
        plantId = id;
        PlantInfo info = GameDataManager.instance.GetPlantInfo(plantId);

        if (info != null)
        {
            if (nameText != null)
                nameText.text = info.name;
        }
    }
}
