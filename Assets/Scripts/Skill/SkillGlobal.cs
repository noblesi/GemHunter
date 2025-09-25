using UnityEngine;

public class SkillGlobal : SkillBase
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
            projectile.GetComponent<ProjectileGlobal>().SetUp(this, CalculateDamage());

            isSkillAvailable = false;
            currentCooldownTime = Time.time;
        }
    }
}
