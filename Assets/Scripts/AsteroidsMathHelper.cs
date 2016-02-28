// Created: Jan 25, 2016
using UnityEngine;
using System.Collections;

public static class AsteroidsMathHelper {

    /// <summary>
    /// Generate a random direction on X-Z plane
    /// </summary>
    /// <returns>unit direction on X-Z plane returned</returns>
    public static Vector3 randomDirectionXZ()
    {
        var speedAngle = Random.Range(0.0f, 360.0f);
        return Quaternion.AngleAxis(speedAngle, Vector3.up) * Vector3.forward;
    }
}
