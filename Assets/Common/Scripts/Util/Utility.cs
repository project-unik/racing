using UnityEngine;
using System;
using System.Collections;

public static class Utility
{
    /// <summary>
    /// Eases a <see cref="float"/> value between <code><paramref name="start"/></code> and <code><paramref name="start"/> + <paramref name="change"/></code>.
    /// Equivalent to <see cref="EaseInOut(float, float, float, float)"/> with <code>time = <paramref name="percentage"/></code> and <code>duration = 1</code>.
    /// </summary>
    /// <param name="start">value at <code><paramref name="percentage"/> = 0</code></param>
    /// <param name="change">difference to <code><paramref name="start"/></code> at <code><paramref name="percentage"/> = 1</code></param>
    /// <param name="percentage">progress of the ease-in-out method; automatically clamped to [0; 1]</param>
    /// <returns>
    /// An eased value between <code><paramref name="start"/></code> and <code><paramref name="start"/> + <paramref name="change"/></code>,
    /// dependent on <code><paramref name="percentage"/></code>.
    /// </returns>
    public static float EaseInOut(float start, float change, float percentage)
    {
        percentage = percentage.Clamp(0, 1);
        return EaseInOut(percentage, start, change, 1);
    }

    /// <summary>
    /// Eases a <see cref="float"/> value between <code><paramref name="start"/></code> and <code><paramref name="start"/> + <paramref name="change"/></code>.
    /// </summary>
    /// <param name="time">progress of the ease-in-out method</param>
    /// <param name="start">value at <code><paramref name="time"/> = 0</code></param>
    /// <param name="change">difference to <code><paramref name="start"/></code> at <code><paramref name="time"/> = <paramref name="duration"/></code></param>
    /// <param name="duration">duration of the whole ease-in-out method</param>
    /// <returns>
    /// An eased value between <code><paramref name="start"/></code> and <code><paramref name="start"/> + <paramref name="change"/></code>, 
    /// dependent on the ratio of <code><paramref name="time"/></code> and <code><paramref name="duration"/></code>.
    /// </returns>
    public static float EaseInOut(float time, float start, float change, float duration)
    {
        time /= duration / 2;
        if (time < 1) return change / 2 * time * time + start;
        time--;
        return -change / 2 * (time * (time - 2) - 1) + start;
    }

    /// <summary>
    /// Eases a <see cref="Vector3"/> between <code><paramref name="from"/></code> and <code><paramref name="to"/></code>.
    /// Equivalent to <see cref="EaseInOut(float, Vector3, Vector3, float)"/> with <code>time = <paramref name="percentage"/></code> and <code>duration = 1</code>.
    /// </summary>
    /// <param name="from">value at <code><paramref name="percentage"/> = 0</code></param>
    /// <param name="to">value at <code><paramref name="percentage"/> = 1</code></param>
    /// <param name="percentage">progress of the ease-in-out method; automatically clamped to [0; 1]</param>
    /// <returns>
    /// An eased <see cref="Vector3"/> between <code><paramref name="from"/></code> and <code><paramref name="to"/></code>,
    /// dependent on <code><paramref name="percentage"/></code>.
    /// </returns>
    public static Vector3 EaseInOut(Vector3 from, Vector3 to, float percentage)
    {
        percentage = percentage.Clamp(0, 1);
        return EaseInOut(percentage, from, to, 1);
    }

    /// <summary>
    /// Eases a <see cref="Vector3"/> between <code><paramref name="from"/></code> and <code><paramref name="to"/></code>.
    /// </summary>
    /// <param name="time">progress of the ease-in-out method</param>
    /// <param name="from">value at <code><paramref name="time"/> = 0</code></param>
    /// <param name="to">value at <code><paramref name="time"/> = <paramref name="duration"/></code></param>
    /// <param name="duration">duration of the whole ease-in-out method</param>
    /// <returns>
    /// An eased <see cref="Vector3"/> between <code><paramref name="from"/></code> and <code><paramref name="to"/></code>,
    /// dependent on the ratio of <code><paramref name="time"/></code> and <code><paramref name="duration"/></code>.
    /// </returns>
    public static Vector3 EaseInOut(float time, Vector3 from, Vector3 to, float duration)
    {
        float xDiff = to.x - from.x;
        float x = EaseInOut(time, from.x, xDiff, duration);

        float yDiff = to.y - from.y;
        float y = EaseInOut(time, from.y, yDiff, duration);

        float zDiff = to.z - from.z;
        float z = EaseInOut(time, from.z, zDiff, duration);

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Calulates the angle between two <see cref="Vector3"/>, <paramref name="from"/> and <paramref name="to"/>,
    /// using <paramref name="normal"/> as a normal.
    /// </summary>
    /// <param name="from">First vector</param>
    /// <param name="to">Second vector</param>
    /// <param name="normal">Vector acting as normal</param>
    /// <returns>The signed angle between <paramref name="from"/> and <paramref name="to"/> 
    /// when projected on the plane described by <paramref name="normal"/>.</returns>
    public static float SignedAngle(Vector3 from, Vector3 to, Vector3 normal)
    {
        
        float angle = Vector3.Angle(from, to);
        float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(from, to)));

        return angle * sign;
    }
}

public static class UtilityExtensions
{
    /// <summary>
    /// Clamps this <see cref="int"/> between <code>minimum</code> and <code>maximum</code>.
    /// Equivalent to <see cref="Mathf.Clamp(int, int, int)"/> with <code>value = this</code>, <code>min = <paramref name="minimum"/></code> and <code>max = <paramref name="maximum"/></code>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minimum">minimum value</param>
    /// <param name="maximum">maximum value</param>
    /// <returns>A value clamped to [<paramref name="minimum"/>; <paramref name="maximum"/>].</returns>
    public static float Clamp(this int value, int minimum, int maximum)
    {
        return Mathf.Clamp(value, minimum, maximum);
    }

    /// <summary>
    /// Clamps this <see cref="float"/> between <code>minimum</code> and <code>maximum</code>.
    /// Equivalent to <see cref="Mathf.Clamp(float, float, float)"/> with <code>value = this</code>, <code>min = <paramref name="minimum"/></code> and <code>max = <paramref name="maximum"/></code>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minimum">minimum value</param>
    /// <param name="maximum">maximum value</param>
    /// <returns>A value clamped to [<paramref name="minimum"/>; <paramref name="maximum"/>].</returns>
    public static float Clamp(this float value, float minimum, float maximum)
    {
        return Mathf.Clamp(value, minimum, maximum);
    }

    /// <summary>
    /// Eases this <see cref="float"/> between <code>this</code> and <code>this + <paramref name="change"/></code>.
    /// Equivalent to <see cref="Utility.EaseInOut(float, float, float)"/> with <code>start = this</code>, <code>change = <paramref name="change"/></code> and <code>percentage = <paramref name="percentage"/></code>.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="change">difference to <code>this</code> at <code><paramref name="percentage"/> = 1</code></param>
    /// <param name="percentage">progress of the ease-in-out method</param>
    /// <returns>An eased value between <code>this</code> and <code>this + <paramref name="change"/></code>, dependent on <code><paramref name="percentage"/></code>.</returns>
    public static float EaseInOut(this float start, float change, float percentage)
    {
        return Utility.EaseInOut(start, change, percentage);
    }

    /// <summary>
    /// Eases this <see cref="float"/> between <code>this</code> and <code>this + <paramref name="change"/></code>.
    /// Equivalent to <see cref="Utility.EaseInOut(float, float, float, float)"/> with <code>start = this</code>,
    /// <code>change = <paramref name="change"/></code>, <code>time = <paramref name="time"/></code> and
    /// <code>duration = <paramref name="duration"/></code>.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="time">progress of the ease-in-out method</param>
    /// <param name="change">difference to <code>this</code> at <code><paramref name="time"/> = <paramref name="duration"/></code></param>
    /// <param name="duration">duration of the whole ease-in-out method</param>
    /// <returns>
    /// An eased value between <code>this</code> and <code>this + <paramref name="change"/></code>, 
    /// dependent on the ratio of <code><paramref name="time"/></code> and <code><paramref name="duration"/></code>.
    /// </returns>
    public static float EaseInOut(this float start, float time, float change, float duration)
    {
        return Utility.EaseInOut(time, start, change, duration);
    }

    /// <summary>
    /// Eases this <see cref="Vector3"/> between <code>this</code> and <code><paramref name="to"/></code>.
    /// Equivalent to <see cref="Utility.EaseInOut(Vector3, Vector3, float)"/> with <code>from = this</code>,
    /// <code>to = <paramref name="to"/></code> and <code>percentage = <paramref name="percentage"/></code>.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to">value at <code><paramref name="percentage"/> = 1</code></param>
    /// <param name="percentage">progress of the ease-in-out method; automatically clamped to [0; 1]</param>
    /// <returns>
    /// An eased <see cref="Vector3"/> between <code>this</code> and <code><paramref name="to"/></code>,
    /// dependent on <code><paramref name="percentage"/></code>.
    /// </returns>
    public static Vector3 EaseInOut(this Vector3 from, Vector3 to, float percentage)
    {
        return Utility.EaseInOut(from, to, percentage);
    }

    /// <summary>
    /// Eases this <see cref="Vector3"/> between <code>this</code> and <code><paramref name="to"/></code>.
    /// Equivalent to <see cref="Utility.EaseInOut(float, Vector3, Vector3, float)"/> with <code>from = this</code>,
    /// <code>to = <paramref name="to"/></code>, <code>time = <paramref name="time"/></code> and
    /// <code>duration = <paramref name="duration"/></code>.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="time">progress of the ease-in-out method</param>
    /// <param name="to">value at <code><paramref name="time"/> = <paramref name="duration"/></code></param>
    /// <param name="duration">duration of the whole ease-in-out method</param>
    /// <returns>
    /// An eased <see cref="Vector3"/> between <code>this</code> and <code><paramref name="to"/></code>,
    /// dependent on the ratio of <code><paramref name="time"/></code> and <code><paramref name="duration"/></code>.
    /// </returns>
    /// <returns></returns>
    public static Vector3 EaseInOut(this Vector3 from, float time, Vector3 to, float duration)
    {
        return Utility.EaseInOut(time, from, to, duration);
    }
}
