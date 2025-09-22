using UnityEngine;

public class EnemyBase : EntityBase
{
    [SerializeField]
    private Transform hudPoint;
    [SerializeField]
    private GameObject uiPrefab;

    private void Awake()
    {
        SetUp();
    }

    protected override void SetUp()
    {
        Stats.GetStat(StatType.HP).BonusValue = 50 * (Stats.level - 1);

        base.SetUp();
    }

    public void Initialize(Transform parent)
    {
        GameObject clone = Instantiate(uiPrefab, parent);
        clone.transform.localScale = Vector3.one;
        clone.GetComponent<FollowTargetUI>().SetUp(hudPoint);
        clone.GetComponentInChildren<UIHP>().SetUp(this);
    }
}
