using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private SkillGad skillGad;
    [SerializeField]
    private Transform skillSpawnPoint;

    private PlayerBase owner;

    private void Awake()
    {
        owner = GetComponent<PlayerBase>();
        skillGad.SetUp(owner, skillSpawnPoint);
    }

    private void Update()
    {
        if (owner.Target == null || owner.IsMoved == true) return;

        skillGad.OnSkill();
    }
}
