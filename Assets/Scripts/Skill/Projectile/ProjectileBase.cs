using System.Diagnostics;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected MovementRigidbody2D movementRigidbody2D;

    public virtual void SetUp(EntityBase target, float damage)
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();

        GetComponent<ScaleEffect>().Play(transform.localScale * 0.2f, transform.localScale);

        GetComponent<ProjectileCollision2D>().SetUp(target, damage);
    }

    public virtual void SetUp(EntityBase target, float damage, int maxCount, int index)
    {
        SetUp(target, damage);
    }

    private void Update()
    {
        Process();
    }

    public abstract void Process();
}
