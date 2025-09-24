using UnityEngine;

public class ProjectileLightningStrike : ProjectileGlobal
{
    [SerializeField]
    private GameObject projectile;

    public override void SetUp(SkillBase skillBase, float damage)
    {
        base.SetUp(skillBase, damage);
    }

    public override void Process()
    {
        for(int i = 0; i < EnemySpawner.Enemies.Count; ++i)
        {
            if (EnemySpawner.Enemies[i] != null) continue;

            Instantiate(projectile, EnemySpawner.Enemies[i].MiddlePoint, Quaternion.identity);
            TakeDamage(EnemySpawner.Enemies[i]);
        }

        Destroy(gameObject);
    }
}
