using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OwnedCardUI : MonoBehaviour
{
    [Header("식물 ID 설정")]
    public int plantId;

    [SerializeField] TextMeshProUGUI plantNameText;
    [SerializeField] Image cardImage;
    [SerializeField] GameObject selectedBadge;

    void OnEnable()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        PlantInfo info = GameDataManager.instance.GetPlantInfo(plantId);
        if (info == null) return;

        plantNameText.text = info.name;

        bool selected = PlayerInventory.instance.IsSelected(plantId);
        if (selectedBadge != null) selectedBadge.SetActive(selected);
    }

    public void OnClickCard()
    {
        if (PlayerInventory.instance.SelectPlant(plantId))
        {
            RefreshUI();
        }
    }
}
