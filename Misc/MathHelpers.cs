using UnityEngine;

public static class MathHelpers
{
    public static Vector3 PickRandomPositionInRadius(Vector3 origin, float minRadius, float maxRadius)
    {
        float radius = Random.Range(minRadius, maxRadius);
        float direction = Random.Range(0, 360);

        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(direction) * radius;
        position.z = Mathf.Sin(direction) * radius;

        return position + origin;
    }
}