using System.Linq;
using UnityEngine;

public abstract class SkillBase
{
    protected SkillTemplate skillTemplate;
    protected PlayerBase owner;
    protected Transform spawnPoint;
    protected int currentLevel = 0;
    protected float currentCooldownTime = 0;
    protected bool isSkillAvailable = false;

    public string SkillName => skillTemplate.skillName;
    public SkillType SkillType => skillTemplate.skillType;
    public SkillElement Element => skillTemplate.element;
    public string Description => skillTemplate.description;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLevel => currentLevel == skillTemplate.maxLevel;

    private Stat[] stats;
    public Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);

    public virtual void SetUp(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        this.skillTemplate = skillTemplate;
        this.owner = owner;
        this.spawnPoint = spawnPoint;

        if(SkillType != SkillType.Buff)
        {
            stats = new Stat[skillTemplate.attackBaseStats.Count];
            for(int i = 0; i < stats.Length; ++i)
            {
                stats[i] = new Stat();
                stats[i].CopyData(skillTemplate.attackBaseStats[i]);
            }
        }
    }

    public void TryLevelUp()
    {
        if (IsMaxLevel)
        {
            Logger.Log($"[{SkillName}] 스킬 최고 레벨 도달");
            return;
        }

        currentLevel++;

        OnLevelUp();
    }

    public void IsSkillAvailable()
    {
        if (CurrentLevel == 0 || SkillType == SkillType.Buff) return;

        if(Time.time - currentCooldownTime > GetStat(StatType.CooldownTime).Value)
        {
            isSkillAvailable = true;
        }
    }

    public abstract void OnLevelUp();
    public abstract void OnSkill();
}
