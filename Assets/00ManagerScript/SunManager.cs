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
    }

    public bool UseSun(float amount)
    {
        if (currentSun >= amount)
        {
            currentSun -= amount;
            return true;
        }
        return false;
    }
}
