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

    public static float GetAngleFromPosition(Vector2 owner, Vector2 target)
    {
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        return degree;
    }

    public static float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }

    public static Vector2 GetNewPoint(Vector3 start, float angle, float r)
    {
        angle = DegreeToRadian(angle);

        Vector2 position = Vector2.zero;
        position.x = Mathf.Cos(angle) * r + start.x;
        position.y = Mathf.Sin(angle) * r + start.y;

        return position;
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * t;
    }

    public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p1 = Lerp(a, b, t);
        Vector2 p2 = Lerp(b, c, t);

        return Lerp(p1, p2, t);
    }

    public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        Vector2 p1 = QuadraticCurve(a, b, c, t);
        Vector2 p2 = QuadraticCurve(b, c, d, t);

        return Lerp(p1, p2, t);
    }
}
