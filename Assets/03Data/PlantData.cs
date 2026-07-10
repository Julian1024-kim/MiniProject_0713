using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class PlantInfo
{
    public int id;
    public string name;
    public int health;         // 체력
    public int damage;         // 공격력
    public float attackSpeed;  // 공격 주기(초)
    public int sunCost;        // 심는 데 필요한 태양 개수
    public float rechargeTime; // 재사용 대기시간(초)
    public bool canAttack;     // 호두같은놈

    public PlantInfo(int id, string name, int health, int damage, float attackSpeed, int sunCost, float rechargeTime, bool canAttack)
    {
        this.id = id;
        this.name = name;
        this.health = health;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.sunCost = sunCost;
        this.rechargeTime = rechargeTime;
        this.canAttack = canAttack;

    }

    public PlantInfo Clone()
    {
        return new PlantInfo(id, name, health, damage, attackSpeed, sunCost, rechargeTime, canAttack);
    }
}

[CreateAssetMenu(fileName = "PlantData", menuName = "Data/Plant")]
public class PlantData : ScriptableObject
{
    public List<PlantInfo> plantList = new List<PlantInfo>();
}