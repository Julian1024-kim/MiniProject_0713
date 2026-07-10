using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isOccupied = false;
    public GameObject plantOnCell;
    private SpriteRenderer sr;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    // ¸¶¿ì½º ¼¿À§¿¡ ¿Ã·ÈÀ»¶§ ±ôºý±ôºý
    void OnMouseEnter()
    {
        sr.color = new Color(1f, 1f, 1f, 0.5f);
    }

    void OnMouseExit()
    {
        sr.color = new Color(1f, 1f, 1f, 1f);
    }

    // 3. Å¬¸¯ÇßÀ» ¶§
    void OnMouseDown()
    {
        if (!isOccupied && PlacementManager.instance != null)
        {
            PlacementManager.instance.PlacePlant(this);
        }
    }
}