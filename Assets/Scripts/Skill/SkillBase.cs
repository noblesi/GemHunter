using UnityEngine;

public abstract class SkillBase
{
    protected SkillTemplate skillTemplate;
    protected PlayerBase owner;
    protected int currentLevel = 0;

    public string SkillName => skillTemplate.skillName;
    public SkillType SkillType => skillTemplate.skillType;
    public SkillElement Element => skillTemplate.element;
    public string Description => skillTemplate.description;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLevel => currentLevel == skillTemplate.maxLevel;

    public virtual void SetUp(SkillTemplate skillTemplate, PlayerBase owner)
    {
        this.skillTemplate = skillTemplate;
        this.owner = owner;
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

    public abstract void OnLevelUp();
    public abstract void OnSkill();
}
