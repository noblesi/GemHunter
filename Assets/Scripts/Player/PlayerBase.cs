using UnityEngine;

public class PlayerBase : EntityBase
{
    public bool IsMoved { get; set; } = false;

    private void Awake()
    {
        base.SetUp();
    }
}
