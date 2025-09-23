using UnityEngine;

public class PlayerBase : EntityBase
{
    [SerializeField]
    private FollowTarget targetMask;

    public bool IsMoved { get; set; } = false;

    private void Awake()
    {
        base.SetUp();
    }

    private void Update()
    {
        if (Target == null) targetMask.gameObject.SetActive(false);

        SearchTarget();
        Recovery();
    }

    private void SearchTarget()
    {
        float closestDistSqr = Mathf.Infinity;

        foreach(var entity in EnemySpawner.Enemies)
        {
            float distance = (entity.transform.position - transform.position).sqrMagnitude;
            if(distance < closestDistSqr)
            {
                closestDistSqr = distance;
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if(Target != null)
        {
            targetMask.SetTarget(Target.transform);
            targetMask.transform.position = Target.transform.position;
            targetMask.gameObject.SetActive(true);
        }
    }

    private void Recovery()
    {
        if (Stats.CurrentHP.DefaultValue < Stats.GetStat(StatType.HP).Value)
            Stats.CurrentHP.DefaultValue += Time.deltaTime * Stats.GetStat(StatType.HPRecovery).Value;
        else
            Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
        
    }
}
