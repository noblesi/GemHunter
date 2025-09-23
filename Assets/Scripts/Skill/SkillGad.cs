using System.Collections;
using UnityEngine;

public class SkillGad : MonoBehaviour
{
    [SerializeField]
    private ProjectileGad projectile;

    private float currentCooldownTime;
    private Transform spawnPoint;
    private PlayerBase owner;
    private WaitForSeconds attackRate = new WaitForSeconds(0.05f);

    public void SetUp(PlayerBase owner, Transform spawnPoint)
    {
        this.owner = owner;
        this.spawnPoint = spawnPoint;
    }

    public void OnSkill()
    {
        if (Time.time - currentCooldownTime > owner.Stats.GetStat(StatType.CooldownTime).Value)
        {
            StartCoroutine(nameof(SpawnProjectile));

            currentCooldownTime = Time.time;
        }
    }

    private IEnumerator SpawnProjectile()
    {
        int projectileCount = 0;
        while(projectileCount < (int)owner.Stats.GetStat(StatType.ProjectileCount).Value)
        {
            var result = CalculateDamage();
            ProjectileGad gad = Instantiate(projectile, spawnPoint.position, Quaternion.identity);
            gad.SetUp(owner, owner.Target, result.Item1, result.Item2);

            projectileCount++;

            yield return attackRate;
        }
    }

    private (float, bool) CalculateDamage()
    {
        bool isCriticalHit = Random.value < owner.Stats.GetStat(StatType.CriticalChance).Value;
        float damage = owner.Stats.GetStat(StatType.Damage).Value;

        if (isCriticalHit)
            return (damage * owner.Stats.GetStat(StatType.CriticalMultiplier).Value, isCriticalHit);
        else
            return (damage, isCriticalHit);
    }
}
