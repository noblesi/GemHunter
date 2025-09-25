using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private SkillGad skillGad;
    [SerializeField]
    private Transform skillSpawnPoint;
    [SerializeField]
    private UISkillList uiSkillList;
    [SerializeField]
    private UISelectSkill uiSelectSkill;
    [SerializeField]
    private GameController gameController;

    private PlayerBase owner;

    private Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    private Dictionary<SkillElement, int> elementalCounts = new Dictionary<SkillElement, int>();
    private Dictionary<SkillElement, SkillBase> elementalSkills = new Dictionary<SkillElement, SkillBase>();

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

        var eSkillDict = Resources.LoadAll<SkillTemplate>("ElementalSkills/").ToDictionary(item => item.name, item => item);
        foreach(var item in eSkillDict)
        {
            SkillBase skill = new SkillBuff();
            skill.SetUp(item.Value, owner, skillSpawnPoint);

            elementalCounts.Add(item.Value.element, 0);
            elementalSkills.Add(item.Value.element, skill);

            Logger.Log($"{item.Value.element}, {item.Value.skillName}");
        }

        uiSkillList.SetUp(skillDict, eSkillDict);
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame) StartSelectSkill();

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
            uiSkillList.LevelUp(skill);
            Logger.Log($"Level Up [{skill.SkillName}] {skill.Element}, Lv. {skill.CurrentLevel}");

            elementalCounts[skill.Element]++;

            if(elementalCounts[skill.Element] % 3 == 0)
            {
                elementalSkills[skill.Element].TryLevelUp();
                uiSkillList.LevelUp(elementalSkills[skill.Element]);
                Logger.Log($"{skill.Element} Lv. {elementalSkills[skill.Element].CurrentLevel}");
            }
        }
    }

    public void StartSelectSkill()
    {
        gameController.SetTimeScale(0);

        var randomSkills = GetRandomSkills(skills, 3);
        if(randomSkills == null)
        {
            Logger.Log("더 이상 습득할 수 있는 스킬이 없습니다.");
            return;
        }

        uiSelectSkill.StartSelectSkillUI(this, randomSkills.ToArray());
    }

    public void EndSelectSkill(SkillBase skill)
    {
        LevelUp(skill);
        uiSelectSkill.EndSelectSkillUI();
        gameController.SetTimeScale(1);
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
