using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField]
    protected EntityStats stats;
    [SerializeField]
    private Transform middlePoint;

    public EntityStats Stats => stats;
    public bool IsDead => stats.currentHP <= 0;
    public Vector3 MiddlePoint => middlePoint != null ? middlePoint.position : Vector3.zero;
    public EntityBase Target { get; set; }

    protected virtual void SetUp()
    {
        stats.currentHP = stats.maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        stats.currentHP = stats.currentHP - damage > 0 ? stats.currentHP - damage : 0;

        if(stats.currentHP <= 0)
        {
            // 사망 처리
        }
    }
}
