using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField]
    private EntityStats stats;
    [SerializeField]
    private Transform middlePoint;

    public EntityStats Stats => stats;
    public bool IsDead => Stats.CurrentHP != null && Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f);
    public Vector3 MiddlePoint => middlePoint != null ? middlePoint.position : Vector3.zero;
    public EntityBase Target { get; set; }

    protected virtual void SetUp()
    {
        Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        if (Random.value < Stats.GetStat(StatType.Evasion).Value) return;

        Stats.CurrentHP.DefaultValue -= damage;

        if(Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f))
        {
            // 사망 처리
        }
    }
}
