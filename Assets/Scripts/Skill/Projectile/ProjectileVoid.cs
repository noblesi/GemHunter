using UnityEngine;

public class ProjectileVoid : ProjectileGlobal
{
    [SerializeField]
    private GameObject projectile;

    private float radius = 1f;

    public override void SetUp(SkillBase skillBase, float damage)
    {
        base.SetUp(skillBase, damage);

        Vector3 targetPosition = skillBase.Owner.Target.MiddlePoint;
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        movementRigidbody2D.MoveTo((targetPosition - transform.position).normalized);
    }

    public override void Process()
    {
        base.Process();

        if(Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            for(int i = 0; i < skillBase.GetStat(StatType.ProjectileCount).Value; ++i)
            {
                float angle = 360 / skillBase.GetStat(StatType.ProjectileCount).Value * i;
                Vector3 position = transform.position + Utils.GetPositionFromAngle(radius, angle);
                GameObject clone = Instantiate(projectile, position, Quaternion.identity);

                Vector3 direction = (clone.transform.position - transform.position).normalized;
                clone.GetComponent<MovementRigidbody2D>().MoveTo(direction);
                clone.GetComponent<ProjectileCollision2D>().SetUp(skillBase.Owner.Target, damage);
            }

            currentAttackRate = Time.time;
        }
    }
}
