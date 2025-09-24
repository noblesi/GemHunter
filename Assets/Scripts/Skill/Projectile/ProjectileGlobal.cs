using UnityEngine;

public class ProjectileGlobal : ProjectileBase
{
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private UIDamageText damageText;

    protected SkillBase skillBase;
    protected float currentDuration;
    protected float currentAttackRate = 0;
    protected float damage;

    public override void SetUp(SkillBase skillBase, float damage)
    {
        this.skillBase = skillBase;
        this.damage = damage;
        currentDuration = Time.time;
    }

    public override void Process()
    {
        if (skillBase.GetStat(StatType.Duration) == null) return;

        if(Time.time - currentDuration > skillBase.GetStat(StatType.Duration).Value)
        {
            Destroy(gameObject);
        }
    }

    protected void TakeDamage(EntityBase entity)
    {
        if(hitEffect != null)
        {
            Instantiate(hitEffect, entity.MiddlePoint, Quaternion.identity);
        }
        if(damageText != null)
        {
            UIDamageText clone = Instantiate(damageText, entity.MiddlePoint, Quaternion.identity);
            clone.SetUp(damage.ToString("F0"), Color.white);
        }

        entity.TakeDamage(damage);
    }
}
