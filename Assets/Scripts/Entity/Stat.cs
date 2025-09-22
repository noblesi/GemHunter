using UnityEngine;

[System.Serializable]
public class Stat
{
    public delegate void ValueChangeHandler(Stat stat, float prev, float current);
    public event ValueChangeHandler OnValueChanged;
    public event ValueChangeHandler OnValueMax;
    public event ValueChangeHandler OnValueMin;

    [SerializeField]
    private StatType statType;
    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float minValue;
    [SerializeField]
    private float defaultValue;
    [SerializeField]
    private float bonusValue;

    public StatType StatType => statType;
    public float Value => Mathf.Clamp(defaultValue + bonusValue, minValue, maxValue);

    public float DefaultValue
    {
        get => defaultValue;
        set
        {
            float prev = Value;
            defaultValue = Mathf.Clamp(value, minValue, maxValue);
            TryInvokeValueChangedEvent(prev, Value);
        }
    }

    public float BonusValue
    {
        get => bonusValue;
        set => bonusValue = value;
    }

    private void TryInvokeValueChangedEvent(float prev, float current)
    {
        if(!Mathf.Approximately(prev, current))
        {
            OnValueChanged?.Invoke(this, prev, current);

            if (Mathf.Approximately(current, maxValue)) OnValueMax?.Invoke(this, prev, maxValue);
            else if (Mathf.Approximately(current, minValue)) OnValueMin?.Invoke(this, prev, minValue);
        }
    }
}

public enum StatType { Damage = 0, CooldownTime, CriticalChance, CriticalMultiplier, HP, Evasion, }
