using UnityEngine;

public class GameOverSensor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie"))
        {
          StageManager.instance.TriggerGameOver();
        }
    }
}
