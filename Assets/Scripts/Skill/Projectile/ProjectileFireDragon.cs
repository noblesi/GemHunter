using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireDragon : ProjectileGlobal
{
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private float weight = 14f;
    private float distancePerSecond = 20;
    private float moveTime;
    private float t = 0f;
    private Vector3 start, end;
    private List<EntityBase> entities;

    public override void SetUp(SkillBase skillBase, float damage)
    {
        base.SetUp(skillBase, damage);

        start = new Vector3(0, stageData.CameraLimitMin.y - weight, 0);
        end = new Vector3(0, stageData.CameraLimitMax.y + weight, 0);
        transform.position = start;

        moveTime = Vector3.Distance(end, start) / distancePerSecond;

        entities = new List<EntityBase>(EnemySpawner.Enemies);

        entities.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
    }

    public override void Process()
    {
        if(t < 1)
        {
            t += Time.deltaTime / moveTime;
            transform.localPosition = Vector3.Lerp(start, end, t);
        }
        else
        {
            spriteRenderer.enabled = false;
            if(entities.Count == 0) Destroy(gameObject);
        }

        if (entities.Count == 0) return;

        if (entities[0] == null) entities.RemoveAt(0);
        else
        {
            if (entities[0].transform.position.y <= transform.position.y)
            {
                TakeDamage(entities[0]);
                entities.RemoveAt(0);
            }
        }
    }
}
