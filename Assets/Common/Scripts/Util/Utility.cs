using UnityEngine;
using System;
using System.Collections;

public static class Utility
{
    public static float easeInOut(float start, float change, float percentage)
    {
        if (percentage < 0 || percentage > 1) throw new ArgumentException(percentage.ToString(), "percentage");
        return easeInOut(percentage, start, change, 1);
    }

    public static float easeInOut(float time, float start, float change, float duration)
    {
        time /= duration / 2;
        if (time < 1) return change / 2 * time * time + start;
        time--;
        return -change / 2 * (time * (time - 2) - 1) + start;
    }

    public static Vector3 easeInOut(Vector3 from, Vector3 to, float percentage)
    {
        float xDiff = to.x - from.x;
        float x = easeInOut(from.x, xDiff, percentage);

        float yDiff = to.y - from.y;
        float y = easeInOut(from.y, yDiff, percentage);

        float zDiff = to.z - from.z;
        float z = easeInOut(from.z, zDiff, percentage);

        return new Vector3(x, y, z);
    }

    public static Vector3 easeInOut(float time, Vector3 from, Vector3 to, float duration)
    {
        float xDiff = to.x - from.x;
        float x = easeInOut(time, from.x, xDiff, duration);

        float yDiff = to.y - from.y;
        float y = easeInOut(time, from.y, yDiff, duration);

        float zDiff = to.z - from.z;
        float z = easeInOut(time, from.z, zDiff, duration);

        return new Vector3(x, y, z);
    }
}

public static class UtilityExtensions
{
    public static float easeInOut(this float start, float change, float percentage)
    {
        return Utility.easeInOut(start, change, percentage);
    }

    public static float easeInOut(this float start, float time, float change, float duration)
    {
        return Utility.easeInOut(time, start, change, duration);
    }

    public static Vector3 easeInOut(this Vector3 from, Vector3 to, float percentage)
    {
        return Utility.easeInOut(from, to, percentage);
    }

    public static Vector3 easeInOut(this Vector3 from, float time, Vector3 to, float duration)
    {
        return Utility.easeInOut(time, from, to, duration);
    }
}
