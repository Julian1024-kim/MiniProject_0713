using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreCard : MonoBehaviour
{
    [Header("식물 ID 설정")]
    public int plantId;

    [SerializeField] TextMeshProUGUI plantNameText;
    [SerializeField] TextMeshProUGUI sunCostText;
    [SerializeField] Button buyButton;
    [SerializeField] GameObject owned; // 보유중 표시 오브젝트

    void OnEnable()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        PlantInfo info = GameDataManager.instance.GetPlantInfo(plantId);
        if (info == null) return;

        plantNameText.text = info.name;
        sunCostText.text = $"Sun {info.sunCost}";

        bool owned = PlayerInventory.instance.IsOwned(plantId);
        buyButton.interactable = !owned;
        if (this.owned != null) this.owned.SetActive(owned);
    }

    public void OnClickBuy()
    {
        if (PlayerInventory.instance.BuyPlant(plantId))
        {
            RefreshUI();
        }
    }
}
