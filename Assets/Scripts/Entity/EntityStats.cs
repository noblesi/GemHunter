using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    [Header("Level, Exp")]
    public int level;
    public float exp;

    [Header("Attack")]
    public float damage;
    public float cooldownTime;
    public float criticalChance;
    public float criticalMultiplier;

    [Header("Defense")]
    public float currentHP;
    public float maxHP;
    public float evasion;
}
