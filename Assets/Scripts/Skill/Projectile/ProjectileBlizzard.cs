using UnityEngine;

public class ProjectileBlizzard : ProjectileGlobal
{
    public override void Process()
    {
        base.Process();

        transform.position = skillBase.Owner.transform.position;

        if(Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            for(int i = 0; i < EnemySpawner.Enemies.Count; ++i)
            {
                if (EnemySpawner.Enemies[i] == null) continue;

                TakeDamage(EnemySpawner.Enemies[i]);
            }

            currentAttackRate = Time.time;
        }
    }
}
