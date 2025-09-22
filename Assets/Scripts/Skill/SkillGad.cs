using UnityEngine;

public class SkillGad : MonoBehaviour
{
    [SerializeField]
    private ProjectileGad projectile;

    private float currentCooldownTime;
    private Transform spawnPoint;
    private PlayerBase owner;

    public void SetUp(PlayerBase owner, Transform spawnPoint)
    {
        this.owner = owner;
        this.spawnPoint = spawnPoint;
    }

    public void OnSkill()
    {
        if (Time.time - currentCooldownTime > owner.Stats.GetStat(StatType.CooldownTime).Value)
        {
            var result = CalculateDamage();
            ProjectileGad gad = Instantiate(projectile, spawnPoint.position, Quaternion.identity);
            gad.SetUp(owner.Target, result.Item1, result.Item2);

            currentCooldownTime = Time.time;
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
