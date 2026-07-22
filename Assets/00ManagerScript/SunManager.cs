using UnityEngine;

public class SunManager : MonoBehaviour
{
    public static SunManager instance;

    public float currentSun = 50;

    void Awake() => 
        instance = this;

    public void AddSun(int amount)
    {
        currentSun += amount;
        Debug.Log($"ÇöÀç ÇÞºû: {currentSun}");
    }

    public bool UseSun(float amount)
    {
        if (currentSun >= amount)
        {
            currentSun -= amount;
            Debug.Log($"ÇÞºû »ç¿ë! ³²Àº ÇÞºû: {currentSun}");
            return true;
        }
        return false;
    }
}
