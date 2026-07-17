using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    public WorldData worldData;
    public int stageIndex;
    public TextMeshProUGUI buttonText;

    public void SetUp(WorldData world, int index)
    {
        worldData = world;
        stageIndex = index;

        buttonText.text = $"{world.worldName} - {index + 1}";
    }

    public void OnClick()
    {
        StageManager.instance.LoadAndStartStage(worldData, stageIndex);
    }
}
