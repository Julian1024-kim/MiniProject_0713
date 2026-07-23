using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class PlantInfo
{
    public int id;
    public string name;
    public float health;         // 체력
    public float damage;         // 공격력
    public float attackSpeed;  // 공격 주기(초)
    public float sunCost;        // 심는 데 필요한 태양 개수
    public float rechargeTime; // 재사용 대기시간(초)
    public bool canAttack;     // 호두,체리폭탄용
    public int coinPrice;


    public PlantInfo(int id, string name, float health, float damage, float attackSpeed, float sunCost, float rechargeTime, bool canAttack, int coinPrice)
    {
        this.id = id;
        this.name = name;
        this.health = health;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.sunCost = sunCost;
        this.rechargeTime = rechargeTime;
        this.canAttack = canAttack;
        this.coinPrice = coinPrice;
    }

    public PlantInfo Clone()
    {
        return new PlantInfo(id, name, health, damage, attackSpeed, sunCost, rechargeTime, canAttack,coinPrice);
    }
}

[CreateAssetMenu(fileName = "PlantData", menuName = "Data/Plant")]
public class PlantData : ScriptableObject
{
    public List<PlantInfo> plantList = new List<PlantInfo>();
}