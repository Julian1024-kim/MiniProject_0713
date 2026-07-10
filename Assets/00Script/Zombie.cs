using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Zombie : MonoBehaviour
{
    public enum ZombieState { Walk, Eat, Die, Action }
    public ZombieState currentState = ZombieState.Walk;

    [Header("데이터 연결")]
    public int zombieId;

    [Header("사거리 설정")]
    public float detectionRange = 6f; // 식물감지사거리
    public LayerMask plantLayer; 

    // 공통 스탯
    public float speed;
    public int health;
    public int damage;
    public float attackSpeed;

    // 특수 능력 스탯
    private ZombieType type;
    private GameObject projectilePrefab;
    private float throwInterval;
    private float rushDistance;
    private float rushDuration;
    private float rushInterval;

    private Plant targetPlant;
    private Coroutine attackCoroutine;
    private Coroutine specialActionCoroutine;
    private bool isPlantInRange = false; // 사거리내 식물존재 여부

    void OnEnable()
    {
        LoadStats();
        ChangeState(ZombieState.Walk);
        targetPlant = null;
        isPlantInRange = false;
    }

    private void LoadStats()
    {
        ZombieInfo info = GameDataManager.instance.GetZombieInfo(zombieId);
        if (info != null)
        {
            health = info.health;
            damage = info.damage;
            attackSpeed = info.attackSpeed;
            speed = info.moveSpeed;

            type = info.type;
            projectilePrefab = info.projectilePrefab;
            throwInterval = info.throwInterval;
            rushDistance = info.rushDistance;
            rushDuration = info.rushDuration;
            rushInterval = info.rushInterval;

            if (type != ZombieType.Normal)
            {
                if (specialActionCoroutine != null) StopCoroutine(specialActionCoroutine);
                specialActionCoroutine = StartCoroutine(SpecialActionRoutine());
            }
        }
    }

    public void ChangeState(ZombieState newState)
    {
        if (currentState == ZombieState.Die) return;
        currentState = newState;

        switch (currentState)
        {
            case ZombieState.Walk:
                StopEatRoutine();
                break;
            case ZombieState.Eat:
                if (attackCoroutine == null)
                    attackCoroutine = StartCoroutine(EatRoutine());
                break;
            case ZombieState.Action:
                StopEatRoutine();
                break;
            case ZombieState.Die:
                StopAllCoroutines();
                transform.DOKill();
                SoundManager.instance.PlaySFX(SFXType.Die);
                gameObject.SetActive(false);
                break;
        }
    }

    void Update()
    {
        if (currentState == ZombieState.Walk)
        {
            CheckForPlantInRange();

            if (!isPlantInRange)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
    }

    void CheckForPlantInRange()
    {
        if (type == ZombieType.Thrower)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, detectionRange, plantLayer);

            if (hit.collider != null)
            {
                isPlantInRange = true;
            }
            else
            {
                isPlantInRange = false;
            }
        }
    }

    IEnumerator SpecialActionRoutine()
    {
        while (true)
        {
            if (type == ZombieType.Thrower)
            {
                // 식물이 사거리 안에 있을 때만던짐
                if (isPlantInRange && currentState != ZombieState.Die)
                {
                    ThrowProjectile();
                    yield return new WaitForSeconds(throwInterval);
                }
                else
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else if (type == ZombieType.Rusher)
            {
                yield return new WaitForSeconds(rushInterval);
                if (currentState == ZombieState.Walk)
                {
                    StartCoroutine(RushAction());
                }
            }
            else yield return null;
        }
    }

    void ThrowProjectile()
    {
        if (projectilePrefab != null)
        {
            ObjectPoolManager.instance.SpawnFromPool("ZombiePea", transform.position, Quaternion.identity);
            Debug.Log("좀비가 투사체를 던짐");
        }
    }

    IEnumerator RushAction()
    {
        ChangeState(ZombieState.Action);

        float elapsed = 0;
        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position + Vector3.left * rushDistance;

        while (elapsed < rushDuration)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.5f, plantLayer);
            if (hit.collider != null)
            {
                targetPlant = hit.collider.GetComponent<Plant>();
                ChangeState(ZombieState.Eat);
                yield break;
            }

            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / rushDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ChangeState(ZombieState.Walk);
    }
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0) ChangeState(ZombieState.Die);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plant"))
        {
            targetPlant = collision.gameObject.GetComponent<Plant>();
            if (targetPlant != null) ChangeState(ZombieState.Eat);
        }
    }

    IEnumerator EatRoutine()
    {
        while (currentState == ZombieState.Eat && targetPlant != null)
        {
            if (targetPlant.gameObject.activeSelf)
            {
                SoundManager.instance.PlaySFX(SFXType.Attack);
                targetPlant.TakeDamage(damage);
                yield return new WaitForSeconds(attackSpeed);
            }
            else
            {
                targetPlant = null;
                ChangeState(ZombieState.Walk);
            }
        }
        attackCoroutine = null;
    }

    private void StopEatRoutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * detectionRange);
    }
}
