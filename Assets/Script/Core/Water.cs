using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    float originColor = 0.05f;
    float underWater = 0.10f;
    static public bool inWater;


    void Awake()
    {
        originColor = RenderSettings.fogDensity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FollowCamera")
        {
            RenderSettings.fogDensity = underWater;
            inWater = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "FollowCamera")
        {
            RenderSettings.fogDensity = originColor;
            inWater = false;
        }
    }
}
