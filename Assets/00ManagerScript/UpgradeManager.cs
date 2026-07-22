using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("업그레이드레벨")]
    public int atkLevel = 1;
    public int prodLevel = 1;

    [Header("업그레이드비용")]
    [SerializeField] int baseAtkCost = 200;
    [SerializeField] int baseProdCost = 150;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        LoadUpgradeData();
    }

    public int GetAtkUpgradeCost() => baseAtkCost + (atkLevel - 1) * 150;
    public int GetProdUpgradeCost() => baseProdCost + (prodLevel - 1) * 100;
    public float GetAtkMultiplier() => 1f + (atkLevel - 1) * 0.1f; //공격력
    public float GetprodREduction() => (prodLevel - 1) * 0.5f;// 생산주기(햇빛같은거)

    public void UpgradeAtk()
    {
        int cost = GetAtkUpgradeCost();

        if (CoinManager.instance.SpendCoins(cost))
        {
            atkLevel++;
            SaveUpgradeData();
        }
    }
    public void UpgradeProd()
    {
        int cost = GetProdUpgradeCost();
        if (CoinManager.instance.SpendCoins(cost))
        {
            prodLevel++;
            SaveUpgradeData();
        }
    }

    void SaveUpgradeData()
    {
        PlayerPrefs.SetInt("AtkLevel", atkLevel);
        PlayerPrefs.SetInt("ProdLevel", prodLevel);
        PlayerPrefs.Save();
    }
    void LoadUpgradeData()
    {
        atkLevel = PlayerPrefs.GetInt("AtkLevel", 1);
        prodLevel = PlayerPrefs.GetInt("ProdLever", 1);
    }
}