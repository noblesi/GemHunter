using UnityEngine;

public static class Utils
{
    public static Quaternion RotateToTarget(Vector2 owner, Vector2 target, float weight = 0)
    {
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0, degree - weight);
    }
}
