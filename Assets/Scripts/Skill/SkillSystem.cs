using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private SkillGad skillGad;
    [SerializeField]
    private Transform skillSpawnPoint;

    private PlayerBase owner;

    private Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();

    private void Awake()
    {
        owner = GetComponent<PlayerBase>();
        skillGad.SetUp(owner, skillSpawnPoint);

        var skillDict = Resources.LoadAll<SkillTemplate>("Skills/").ToDictionary(item => item.name, item => item);
        foreach(var item in skillDict)
        {
            SkillBase skill = null;
            if (item.Value.skillType.Equals(SkillType.Buff)) skill = new SkillBuff();
            else if (item.Value.skillType.Equals(SkillType.Emission)) skill = new SkillEmission();
            else if (item.Value.skillType.Equals(SkillType.Sustained)) skill = new SkillSustained();
            else if (item.Value.skillType.Equals(SkillType.Global)) skill = new SkillGlobal();

                skill.SetUp(item.Value, owner, skillSpawnPoint);
            skills.Add(item.Key, skill);

            Logger.Log($"[{skill.SkillName}] Lv. {skill.CurrentLevel}\n{skill.Description}");
        }
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame) SelectSkill();

        foreach(var item in skills)
        {
            if (item.Value.CurrentLevel == 0) continue;

            item.Value.OnSkill();
        }

        if (owner.Target == null || owner.IsMoved == true) return;

        skillGad.OnSkill();

        foreach(var item in skills)
        {
            item.Value.IsSkillAvailable();
        }
    }

    public void LevelUp(SkillBase skill)
    {
        if (skills.ContainsValue(skill))
        {
            skill.TryLevelUp();
            Logger.Log($"Level Up [{skill.SkillName}] {skill.Element}, Lv. {skill.CurrentLevel}");
        }
    }

    public void SelectSkill()
    {
        var randomSkills = GetRandomSkills(skills, 3);
        if(randomSkills == null)
        {
            Logger.Log("더 이상 습득할 수 있는 스킬이 없습니다.");
            return;
        }

        int index = Random.Range(0, randomSkills.Count);
        LevelUp(randomSkills[index]);
    }

    public List<SkillBase> GetRandomSkills(Dictionary<string, SkillBase> skills, int count = 3)
    {
        var values = new List<SkillBase>(skills.Values.Where(skill => !skill.IsMaxLevel)).ToList();
        var randomSkills = new List<SkillBase>();

        count = values.Count == 0 ? 0 : count;

        if (count == 0) return null;

        for(int i = 0; i < count; ++i)
        {
            int index = Random.Range(0, values.Count);

            randomSkills.Add(values[index]);

            values.RemoveAt(index);
        }

        Logger.Log($"선택 가능한 3개의 스킬\n{randomSkills[0].SkillName}," +
            $"{randomSkills[1].SkillName}, {randomSkills[2].SkillName}");

        return randomSkills;
    }
}
