using UnityEngine;

public class ProjectileStraight : ProjectileBase
{
    public override void SetUp(EntityBase target, float damage)
    {
        base.SetUp(target, damage);

        transform.rotation = Utils.RotateToTarget(transform.position, target.MiddlePoint, 90);

        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);
    }

    public override void Process() { }   
}
