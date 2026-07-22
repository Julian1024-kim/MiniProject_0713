using UnityEngine;

public class Pea : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 20;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > 18f)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            
                float finalDamage = damage * UpgradeManager.instance.GetAtkMultiplier();
                collision.gameObject.GetComponent<Zombie>().TakeDamage(finalDamage);
            
            gameObject.SetActive(false);
        }
    }
}
