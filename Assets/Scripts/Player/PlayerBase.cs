using UnityEngine;

public class PlayerBase : EntityBase
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private FollowTarget targetMark;
    [SerializeField]
    private LevelData levelData;
    [SerializeField]
    private SkillSystem skillSystem;

    private float expAmount = 2f;

    public bool IsMoved { get; set; } = false;
    public float AccumulationExp { get; set; } = 0f;
    public int GEM { get; private set; } = 0;

    private void Awake()
    {
        base.SetUp();

        Stats.CurrentExp.DefaultValue = 0f;
        Stats.CurrentExp.OnValueChanged += IsLevelUP;
        Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[0];
    }

    private void Update()
    {
        if (Target == null) targetMark.gameObject.SetActive(false);

        SearchTarget();
        Recovery();
        UpdateEXP();
    }

    protected override void OnDie()
    {
        gameController.GameOver();
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
            targetMark.SetTarget(Target.transform);
            targetMark.transform.position = Target.transform.position;
            targetMark.gameObject.SetActive(true);
        }
    }

    private void Recovery()
    {
        if (Stats.CurrentHP.DefaultValue < Stats.GetStat(StatType.HP).Value)
            Stats.CurrentHP.DefaultValue += Time.deltaTime * Stats.GetStat(StatType.HPRecovery).Value;
        else
            Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
        
    }

    private void UpdateEXP()
    {
        if (Mathf.Approximately(AccumulationExp, 0f) || skillSystem.IsSelectSkill == true) return;

        float getEXP = AccumulationExp > expAmount ? expAmount : AccumulationExp;
        AccumulationExp -= getEXP;
        Stats.CurrentExp.DefaultValue += getEXP;
    }

    private void IsLevelUP(Stat stat, float prev, float current)
    {
        if (!Mathf.Approximately(Stats.CurrentExp.Value, Stats.GetStat(StatType.Experience).Value)) return;

        Stats.GetStat(StatType.Level).DefaultValue++;

        Stats.CurrentExp.DefaultValue -= Stats.GetStat(StatType.Experience).Value;

        if (Stats.GetStat(StatType.Level).Value < levelData.MaxExperience.Length)
            Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[(int)Stats.GetStat(StatType.Level).Value - 1];
        else
            Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[levelData.MaxExperience.Length - 1];

        skillSystem.StartSelectSkill();
    }

    public void AddGEM()
    {
        GEM++;
    }
}
