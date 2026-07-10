using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager instance;

    [Header("선택된 식물 정보")]
    public PlantInfo selectedPlantData;
    public GameObject selectedPrefab;  

    void Awake()
    {
        instance = this;
    }

    public void SetSelectedPlant(PlantInfo info, GameObject prefab)
    {
        selectedPlantData = info;
        selectedPrefab = prefab;
        Debug.Log($"{info.name} 카드가 선택되었습니다. 가격: {info.sunCost}");
    }

    public void PlacePlant(Cell cell)
    {
        if (selectedPlantData == null || selectedPrefab == null)
        {
            Debug.Log("심을 식물이 선택되지않음");
            return;
        }

        if (cell.isOccupied)
        {
            Debug.Log("이미 식물이 있음");
            return;
        }

        if (SunManager.instance.UseSun(selectedPlantData.sunCost))
        {
            if (ObjectPoolManager.instance == null) return;
            GameObject newPlant = ObjectPoolManager.instance.SpawnFromPool(
            selectedPlantData.name,
            cell.transform.position,
            Quaternion.identity
        );
            if (newPlant == null) return;

            Plant plantScript = newPlant.GetComponent<Plant>();
            if (plantScript != null)
            {
                plantScript.plantId = selectedPlantData.id;
            }

            cell.isOccupied = true;
            cell.plantOnCell = newPlant;

            Debug.Log($"{selectedPlantData.name} 배치 완료");

            // 한번 심으면 해제시킴
            selectedPlantData = null;
            selectedPrefab = null;
        }
        else
        {
            Debug.Log("햇빛이 부족합니다");
        }
    }
}
