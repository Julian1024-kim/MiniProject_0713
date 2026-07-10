using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    public List<int> ownedPlantIds = new List<int>();

    public List<int> selectedPlantIds = new List<int>();
    public int maxSelectCount = 5; // 최대 슬롯 수

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public bool BuyPlant(int plantId)
    {
        if (ownedPlantIds.Contains(plantId))
        {
            Debug.Log("이미 보유 중인 카드입니다.");
            return false;
        }
        ownedPlantIds.Add(plantId);
        Debug.Log($"식물 ID {plantId} 구매 완료");
        return true;
    }

    public bool IsOwned(int plantId) => ownedPlantIds.Contains(plantId);
    public bool IsSelected(int plantId) => selectedPlantIds.Contains(plantId);

    public bool SelectPlant(int plantId)
    {
        if (!IsOwned(plantId)) return false;
        if (IsSelected(plantId))
        {
            Debug.Log("이미 선택된 카드입니다.");
            return false;
        }
        if (selectedPlantIds.Count >= maxSelectCount)
        {
            return false;
        }
        selectedPlantIds.Add(plantId);
        return true;
    }
    public int GetSelectedPlant(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= selectedPlantIds.Count) return -1;
        return selectedPlantIds[slotIndex];
    }

    public void DeselectPlant(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= selectedPlantIds.Count) return;
        selectedPlantIds.RemoveAt(slotIndex);
    }
    public void ToggleSelectPlant(int plantId)
    {
        if (selectedPlantIds.Contains(plantId))
        {
            selectedPlantIds.Remove(plantId);
            Debug.Log($"식물 {plantId} 선택 해제");
        }
        else
        {
            if (selectedPlantIds.Count < maxSelectCount)
            {
                selectedPlantIds.Add(plantId);
                Debug.Log($"식물 {plantId} 선택 완료");
            }
            else
            {
                Debug.Log("최대 5개까지만 선택 가능합니다.");
            }
        }
    }

}
