// Created: Jan 25, 2016
using UnityEngine;
using System.Collections;

/// <summary>
/// Seems that Unity can't GetComponent() a Interface so using Abstract class
/// </summary>
public abstract class ADieableBehavior : MonoBehaviour {
    public bool isDying = false;

    /// <summary>
    /// sets isDying = true and destroy this object
    /// </summary>
    public virtual void Die()
    {
        this.isDying = true;
        DestroyObject(gameObject);
    }
}
