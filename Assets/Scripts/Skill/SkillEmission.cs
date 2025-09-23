using UnityEngine;

public class SkillEmission : SkillBase
{
    private float attackRate = 0.05f;
    private float currentAttackRate = 0;
    private int currentProjectileCount = 0;

    public override void OnLevelUp()
    {
        if (currentLevel <= 1) return;

        skillTemplate.attackBuffStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue;
        });
    }

    public override void OnSkill()
    {
        int maxCount = (int)GetStat(StatType.ProjectileCount).Value;

        if(Time.time - currentAttackRate > attackRate)
        {
            GameObject projectile = GameObject.Instantiate(skillTemplate.projectile, spawnPoint.position, Quaternion.identity);

            if (projectile.TryGetComponent<ProjectileCubicHoming>(out var p))
                p.SetUp(owner.Target, GetStat(StatType.Damage).Value, maxCount, currentProjectileCount);
            else if (projectile.TryGetComponent<ProjectileQuadraticHoming>(out var p2))
                p2.SetUp(owner.Target, GetStat(StatType.Damage).Value, maxCount, currentProjectileCount);
            else projectile.GetComponent<ProjectileBase>().SetUp(owner.Target, GetStat(StatType.Damage).Value);

            currentProjectileCount++;
            currentAttackRate = Time.time;
        }

        if(currentProjectileCount >= maxCount)
        {
            isSkillAvailable = false;
            currentProjectileCount = 0;
            currentCooldownTime = Time.time;
        }
    }
}
