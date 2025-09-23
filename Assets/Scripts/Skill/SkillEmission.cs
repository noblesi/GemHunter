using UnityEngine;

public class SkillEmission : SkillBase
{
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
        if(isSkillAvailable == true)
        {
            GameObject projectile = GameObject.Instantiate(skillTemplate.projectile, spawnPoint.position, Quaternion.identity);
            projectile.GetComponent<ProjectileBase>().SetUp(owner.Target, GetStat(StatType.Damage).Value);

            isSkillAvailable = false;
            currentCooldownTime = Time.time;
        }
    }
}
