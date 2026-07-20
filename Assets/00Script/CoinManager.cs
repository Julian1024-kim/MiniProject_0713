using UnityEditor.Overlays;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    int totalCoins;

    private void Awake()
    {
       if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
       else
        {
            Destroy(gameObject);
        }
    }
    public void AddCoins(int amount)
    {
        totalCoins += amount;
        SaveData();
    }
    public bool SpendCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            SaveData();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
    }
    private void LoadData()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }
    public int GetTotalCoins() => totalCoins;
    
}
