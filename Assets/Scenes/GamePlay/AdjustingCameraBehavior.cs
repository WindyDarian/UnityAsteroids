// Created Jan 25, 2016

using UnityEngine;
using System.Collections;

public class AdjustingCameraBehavior : MonoBehaviour {
    // This script restricts aspect ratio to something

    public float forceAspectRatio = 1.333333f;

	void Start () {
        
        var aspect_original = Camera.main.aspect;
        var difference = (forceAspectRatio - aspect_original);

        if (difference > 0.01f)
        {
            var new_height = aspect_original / forceAspectRatio;
            Camera.main.rect = new Rect(0, (1 - new_height) * .5f, 1, new_height);
        }
        else if (difference < -0.01f)
        {
            var new_width = forceAspectRatio / aspect_original;
            Camera.main.rect = new Rect((1 - new_width) * .5f, 0, new_width, 1);
        }
    }
	
}
