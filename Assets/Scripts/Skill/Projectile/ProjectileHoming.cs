using UnityEngine;

public class ProjectileHoming : ProjectileBase
{
    private EntityBase target;

    public override void SetUp(EntityBase target, float damage)
    {
        base.SetUp(target, damage);

        this.target = target;
    }

    public override void Process()
    {
        if(target == null)
        {
            FindTarget();
            return;
        }

        transform.rotation = Utils.RotateToTarget(transform.position, target.MiddlePoint, 90);

        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);
    }

    private void FindTarget()
    {
        if(EnemySpawner.Enemies.Count > 0)
        {
            target = EnemySpawner.Enemies[0];
            GetComponent<ProjectileCollision2D>().SetTarget(target);
        }
        else Destroy(gameObject);
    }
}
