using System;
using UnityEngine;
using System.Collections.Generic;

public enum ZombieType { Normal, Thrower, Rusher }

[Serializable]
public class ZombieInfo
{
    public int id;
    public string name;
    public ZombieType type;    // 좀비 유형 추가
    public float health;         // 체력
    public float damage;         // 공격력(씹는 데미지)
    public float attackSpeed;  // 공격 주기(초)
    public float moveSpeed;    // 이동 속도

    [Header("Special Ability Settings")]
    // 투사체 좀비용
    public GameObject projectilePrefab;
    public float throwInterval;

    // 돌진 좀비용
    public float rushDistance;
    public float rushDuration;
    public float rushInterval;

    public ZombieInfo(int id, string name, ZombieType type, float health, float damage, float attackSpeed, float moveSpeed)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.health = health;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.moveSpeed = moveSpeed;
    }

    public ZombieInfo Clone()
    {
        ZombieInfo clone = new ZombieInfo(id, name, type, health, damage, attackSpeed, moveSpeed);
        // 특별 능력 데이터도 복제
        clone.projectilePrefab = this.projectilePrefab;
        clone.throwInterval = this.throwInterval;
        clone.rushDistance = this.rushDistance;
        clone.rushDuration = this.rushDuration;
        clone.rushInterval = this.rushInterval;
        return clone;
    }
}

[CreateAssetMenu(fileName = "ZombieData", menuName = "Data/Zombie")]
public class ZombieData : ScriptableObject
{
    public List<ZombieInfo> zombieList = new List<ZombieInfo>();
}
