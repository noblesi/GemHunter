using System.Collections.Generic;
using UnityEngine;

public class SkillSustained : SkillBase
{
    private float distanceToPlayer = 2f;
    private Transform parent;
    private List<GameObject> pickaxs = new List<GameObject>();

    public override void SetUp(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        base.SetUp(skillTemplate, owner, spawnPoint);

        parent = GameObject.Find("Pickaxs").transform;
    }

    public override void OnLevelUp()
    {
        if(currentLevel <= 1)
        {
            AddPickax((int)GetStat(StatType.ProjectileCount).Value);

            int pickaxCount = parent.childCount;
            for(int i = 0; i < pickaxCount; ++i)
            {
                float angle = (360 / pickaxCount) * i;
                Vector3 position = Utils.GetPositionFromAngle(distanceToPlayer, angle);
                parent.GetChild(i).position = parent.position + position;
            }

            return;
        }

        skillTemplate.attackBuffStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue;
        });

        foreach(var item in pickaxs)
        {
            item.GetComponent<ProjectileCollision2D>().SetUp(null, GetStat(StatType.Damage).Value);
        }
    }

    private void AddPickax(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            GameObject clone = GameObject.Instantiate(skillTemplate.projectile, parent);
            clone.GetComponent<ProjectileCollision2D>().SetUp(null, GetStat(StatType.Damage).Value);
            pickaxs.Add(clone);
        }
    }

    public override void OnSkill() { }
}
