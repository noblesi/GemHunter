using System.Linq;
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    [Header("Level, Exp")]
    public int level;
    public float exp;

    /*[Header("Attack")]
    public float damage;
    public float cooldownTime;
    public float criticalChance;
    public float criticalMultiplier;

    [Header("Defense")]
    public float currentHP;
    public float maxHP;
    public float evasion;*/

    [Header("Current Stats")]
    [SerializeField]
    private Stat currentHP;

    [Header("Stats")]
    [SerializeField]
    private Stat[] stats;

    public readonly Stat CurrentHP => currentHP;
    public readonly Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public readonly Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);
}
