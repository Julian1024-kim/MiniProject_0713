using System.Collections;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [Header("데이터 연결")]
    public int plantId;

    public float health;
    public float fireRate;
    public bool _canAttack;

    [Header("사거리 설정 (레이캐스트)")]
    public float detectionRange = 10f; // 식물감지거리
    public LayerMask zombieLayer;

    public Cell currentCell;

    private bool isZombieDetected = false;

    protected virtual void OnEnable()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        PlantInfo info = GameDataManager.instance.GetPlantInfo(plantId);
        if (info != null)
        {
            health = info.health;
            fireRate = info.attackSpeed;
            _canAttack = info.canAttack;

            if (_canAttack)
            {
                StopAllCoroutines();
                StartCoroutine(AttackRoutine());
            }
        }
    }

    void Update()
    {
        if (_canAttack)
        {
            CheckForZombie();
        }
    }

    private void CheckForZombie()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectionRange, zombieLayer);

        if (hit.collider != null)
        {
            isZombieDetected = true;
        }
        else
        {
            isZombieDetected = false;
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (isZombieDetected)
            {
                Shoot();
                yield return new WaitForSeconds(fireRate);
            }
            else
            {
                // 좀비가 없으면 짧게 대기하며 다시 체크
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    protected virtual void Shoot()
    {
        ObjectPoolManager.instance.SpawnFromPool("Pea", transform.position, Quaternion.identity);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * detectionRange);
    }
}