using System.Collections;
using UnityEngine;

public class BombPlant : Plant
{
    [Header("체리폭탄 설정")]
    public Vector2 explosionSize = new Vector2(3f, 3f); // 폭발 범위
    public float explosionDelay = 3f;   // 감지 후 터지기까지 시간
    public GameObject explosionEffect;   // 폭발 이펙트 ( 시간이있다면 )

    private bool isCountingDown = false;
    private bool hasExploded = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        isCountingDown = false;
        hasExploded = false;
    }

    void Update()
    {
        if (hasExploded || isCountingDown) return;

        Collider2D hitZombie = Physics2D.OverlapBox(transform.position, explosionSize, 0f, zombieLayer);

        if (hitZombie != null)
        {
            StartCoroutine(ExplosionTimerRoutine());
        }
    }

    IEnumerator ExplosionTimerRoutine()
    {
        isCountingDown = true;
        Debug.Log("체리폭탄 좀비 감지! 3초 뒤 폭발합니다.");

        yield return new WaitForSeconds(explosionDelay);

        Explode();
    }

    private void Explode()
    {
        hasExploded = true;

        Collider2D[] zombiesInRange = Physics2D.OverlapBoxAll(transform.position, explosionSize, 0f, zombieLayer);

        int damageValue = 0;
        PlantInfo info = GameDataManager.instance.GetPlantInfo(plantId);
        if (info != null) damageValue = info.damage;

        foreach (Collider2D zombie in zombiesInRange)
        {
            Debug.Log($"{zombie.name} 감지됨!");

            Zombie zombieScript = zombie.GetComponent<Zombie>();

            if (zombieScript != null)
            {
                zombieScript.TakeDamage(damageValue);
                Debug.Log($"{zombie.name}에게 {damageValue} 데미지 입힘!");
            }
        }


        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }
    protected override void Shoot() { }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, explosionSize);
    }
}
