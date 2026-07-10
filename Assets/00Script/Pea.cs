using UnityEngine;

public class Pea : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 20;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > 15f)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
            gameObject.SetActive(false);
        }
    }
}
