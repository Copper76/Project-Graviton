using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum Axis
{
    X,
    Y,
    Z
}

public static class Vector3Extensions
{
    public static Vector3 GetAxis(this Vector3 vector, Axis axis)
    {
        switch (axis)
        {
            case Axis.X:
                return new Vector3(vector.x, 0, 0);
            case Axis.Y:
                return new Vector3(0, vector.y, 0);
            case Axis.Z:
                return new Vector3(0, 0, vector.z);
            default:
                throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
        }
    }

    public static Vector3 GetVectorOfScale(float scale)
    {
        return Vector3.one * scale;
    }

    public static Vector3 ClampAxes(this Vector3 vector, Vector3 minVector, Vector3 maxVector)
    {
        vector.x = Mathf.Clamp(vector.x, minVector.x, maxVector.x);
        vector.y = Mathf.Clamp(vector.y, minVector.y, maxVector.y);
        vector.z = Mathf.Clamp(vector.z, minVector.z, maxVector.z);

        return vector;
    }

}

