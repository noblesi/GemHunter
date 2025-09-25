﻿using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectSkillIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image skillIcon;
    [SerializeField]
    private TextMeshProUGUI skillName;
    [SerializeField]
    private TextMeshProUGUI skillDescription;

    private SkillSystem skillSystem;
    private SkillBase skillBase;

    public void SetUp(SkillSystem skillSystem, SkillBase skillBase)
    {
        this.skillSystem = skillSystem;
        this.skillBase = skillBase;
        skillIcon.sprite = skillBase.EnableIcon;
        skillName.text = skillBase.SkillName;
        skillDescription.text = skillBase.Description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillSystem.EndSelectSkill(skillBase);
    }
}
