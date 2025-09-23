using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Buff = 0, }
public enum SkillElement { None = -1, Ice = 100, Fire, Wind, Light, Dark, Count = 5 }

[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillAsset", order = 0)]
public class SkillTemplate : ScriptableObject
{
    [Header("공통")]
    public string skillName;
    public SkillType skillType;
    public SkillElement element;
    public int maxLevel;

    [TextArea(1, 30)]
    public string description;
    public Sprite disableIcon;
    public Sprite enableIcon;

    [Header("버프 스킬")]
    public List<Stat> buffStatList;
}
