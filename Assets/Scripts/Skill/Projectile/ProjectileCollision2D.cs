using UnityEngine;

public enum DestroyType { None = -1, Collision = 0, }

public class ProjectileCollision2D : MonoBehaviour
{
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private UIDamageText damageText;
    [SerializeField]
    private DestroyType destroyType = DestroyType.None;
    [SerializeField]
    private bool isIgnoreWall = false;

    private EntityBase target;
    private float damage;

    public void SetUp(EntityBase target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    public void SetTarget(EntityBase target) => this.target = target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall") && isIgnoreWall == false)
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Enemy") &&
            collision.TryGetComponent<EntityBase>(out var entity))
        {
            if (entity != target) return;
            
            if(hitEffect != null)
            {
                Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
            }
            if(damageText != null)
            {
                UIDamageText clone = Instantiate(damageText, collision.transform.position, Quaternion.identity);
                clone.SetUp(damage.ToString("F0"), Color.white);
            }

            entity.TakeDamage(damage);

            if(destroyType == DestroyType.Collision)
            {
                Destroy(gameObject);
            }
        }

    }
}
