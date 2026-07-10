using UnityEngine;

public class PlantCard : MonoBehaviour
{
    [Header("이 카드가 생성할 식물 정보")]
    public int plantId;            // PlantData 리스트에 등록한ID (1일반, 2빠른놈)
    public GameObject plantPrefab;

    public void OnClickCard()
    {
        PlantInfo info = GameDataManager.instance.GetPlantInfo(plantId);

        if (info != null)
        {
            PlacementManager.instance.SetSelectedPlant(info, plantPrefab);
            Debug.Log($"{info.name} 선택됨! 가격: {info.sunCost}");
        }
    }
}
