using UnityEngine;

public class ProjectileTornado : ProjectileGlobal
{
    [SerializeField]
    private float attackRadius = 2f;
    [SerializeField]
    private LayerMask targetLayer;

    private EntityBase target;

    public override void SetUp(SkillBase skillBase, float damage)
    {
        base.SetUp(skillBase, damage);

        movementRigidbody2D = GetComponent<MovementRigidbody2D>();

        target = skillBase.Owner.Target;
    }

    public override void Process()
    {
        base.Process();

        if(target == null)
        {
            SearchClosestTarget();
            return;
        }

        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);

        if(Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, attackRadius, targetLayer);
            for(int i = 0; i < entities.Length; ++i)
            {
                if (entities[i].TryGetComponent<EntityBase>(out var entity))
                {
                    TakeDamage(entity);
                }
            }

            currentAttackRate = Time.time;
        }
    }

    private void SearchClosestTarget()
    {
        float closestDistSqr = Mathf.Infinity;
        foreach(var entity in EnemySpawner.Enemies)
        {
            float distance = (entity.transform.position - transform.position).sqrMagnitude;
            if(distance < closestDistSqr)
            {
                closestDistSqr = distance;
                target = entity.GetComponent<EntityBase>();
            }
        }
    }
}
