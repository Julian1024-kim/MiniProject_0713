using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrepCard : MonoBehaviour
{
    public int plantId;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject checkMark; // 선택 시 켜질 오브젝트
    [SerializeField] TextMeshProUGUI nameText;

    public void Setup(int id)
    {
        plantId = id;
        PlantInfo info = GameDataManager.instance.GetPlantInfo(id);
        if (info != null)
        {
            nameText.text = info.name;
        }
        RefreshUI();
    }

    public void RefreshUI()
    {
        // PlayerInventory에 이 ID가 선택되어 있는지 확인
        bool isSelected = PlayerInventory.instance.selectedPlantIds.Contains(plantId);
        checkMark.SetActive(isSelected);
    }

    public void OnClickCard()
    {
        if (PlayerInventory.instance.selectedPlantIds.Contains(plantId))
        {
            PlayerInventory.instance.selectedPlantIds.Remove(plantId);
        }
        else
        {
            if (PlayerInventory.instance.selectedPlantIds.Count < PlayerInventory.instance.maxSelectCount)
            {
                PlayerInventory.instance.selectedPlantIds.Add(plantId);
            }
        }

        // 전체 카드 UI 새로고침 (부모에게 알림)
        SendMessageUpwards("UpdateAllCards");
    }
}
