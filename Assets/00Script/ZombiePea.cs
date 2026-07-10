using UnityEngine;

public class ZombiePea : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 20;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -15f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            Plant plant = other.GetComponent<Plant>();
            if (plant != null)
            {
                plant.TakeDamage(damage);
                Debug.Log("식물에게 투사체 적중!");
            }
            gameObject.SetActive(false);
        }
    }
}
