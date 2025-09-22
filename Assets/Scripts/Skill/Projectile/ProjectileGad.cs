using UnityEngine;

public class ProjectileGad : MonoBehaviour
{
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private UIDamageText damageText;

    private MovementRigidbody2D movementRigidbody2D;
    private ScaleEffect scaleEffect;
    private EntityBase target;
    private float damage;
    private bool isCritical;

    public void SetUp(EntityBase target, float damage, bool isCritical = false)
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        scaleEffect = GetComponent<ScaleEffect>();
        this.target = target;
        this.damage = damage;
        this.isCritical = isCritical;

        scaleEffect.Play(transform.localScale * 0.35f, transform.localScale);

        transform.rotation = Utils.RotateToTarget(transform.position, target.MiddlePoint, 90);

        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Enemy") &&
            collision.TryGetComponent<EntityBase>(out var entity))
        {
            if (entity != target) return;

            if(hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            if(damageText != null)
            {
                UIDamageText clone = Instantiate(damageText, transform.position, Quaternion.identity);
                clone.SetUp(damage.ToString("F0"), isCritical ? Color.red : Color.white);
            }

            entity.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
