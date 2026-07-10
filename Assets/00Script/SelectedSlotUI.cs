using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedSlotUI : MonoBehaviour
{
    [Header("ΩΩ∑‘ ¿Œµ¶Ω∫")]
    public int slotIndex;

    [SerializeField] TextMeshProUGUI plantNameText;
    [SerializeField] Image cardImage;

    void OnEnable()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        int plantId = PlayerInventory.instance.GetSelectedPlant(slotIndex);

        if (plantId == -1)
        {
            plantNameText.text = "∫ÒæÓ¿÷¿Ω";
            if (cardImage != null) cardImage.color = Color.gray;
        }
        else
        {
            PlantInfo info = GameDataManager.instance.GetPlantInfo(plantId);
            if (info == null) return;
            plantNameText.text = info.name;
            if (cardImage != null) cardImage.color = Color.white;
        }
    }

    public void OnClickSlot()
    {
        int plantId = PlayerInventory.instance.GetSelectedPlant(slotIndex);
        if (plantId == -1) return;

        PlayerInventory.instance.DeselectPlant(slotIndex);
        RefreshUI();
    }
}
