// Created Jan 25, 2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarpingBehavior : MonoBehaviour
{
    
    //public GameObject clonePrefab;


    public GameBoundary2D boundary;
    public GameObject mirrorPrefab;

    // to achieve smooth warping effect around border
    private List<GameObject> mirrors;
    private List<Vector3> mirrordifferences;


    void Start()
    {
        if (mirrorPrefab)
        {

            mirrors = new List<GameObject>(8);
            for (int i = 0; i< 8; i++)
            {
                mirrors.Add((GameObject)Instantiate(mirrorPrefab, new Vector3(10000, 0, 0), this.transform.rotation));
            }
        }
        
    }

    void Update()
    {
        if (!boundary)
            return;

        var right_vector = Vector3.right;
        var top_vector = Vector3.forward;
        var rigid_body = GetComponent<Rigidbody>();
        var width = boundary.right - boundary.left;
        var height = boundary.top - boundary.down;

        if (rigid_body.position.z > boundary.top + 0.01f)  //&& Vector3.Dot(rigid_body.velocity, top_vector) >= 0)
        {
            // rigid_body.position = new Vector3(rigid_body.position.x, rigid_body.position.y, boundary.down);
            rigid_body.position -= height * top_vector;
        }
        else if (rigid_body.position.z < boundary.down - 0.01f) //&& Vector3.Dot(rigid_body.velocity, top_vector) <= 0)
        {
            //rigid_body.position = new Vector3(rigid_body.position.x, rigid_body.position.y, boundary.top);
            rigid_body.position += height * top_vector;
        }
        else if (rigid_body.position.x > boundary.right + 0.01f) //&& Vector3.Dot(rigid_body.velocity, right_vector) >= 0)
        {
            //rigid_body.position = new Vector3(boundary.left, rigid_body.position.y, rigid_body.position.z);
            rigid_body.position -= width * right_vector;
        }
        else if (rigid_body.position.x < boundary.left -0.01f) //&& Vector3.Dot(rigid_body.velocity, right_vector) <= 0)
        {
            //rigid_body.position = new Vector3(boundary.right, rigid_body.position.y, rigid_body.position.z);
            rigid_body.position += width * right_vector;
        }

        // update wrap mirrors
        if (mirrorPrefab)
        {
            int currentcount = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        mirrors[currentcount].transform.position = transform.position
                            + right_vector * i * width
                            + top_vector * j * height;
                        mirrors[currentcount].transform.rotation = transform.rotation;
                        currentcount += 1;
                    }
                }
            }
        }


    }


    void OnDestroy()
    {
        if (mirrorPrefab)
        {
            foreach (var mir in mirrors)
            {
                GameObject.Destroy(mir);
            }
        }
    }
}
