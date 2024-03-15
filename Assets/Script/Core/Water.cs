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

    /// <summary>
    /// 플레이어가 물속에 들어왔을때
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FollowCamera")
        {
            RenderSettings.fogDensity = underWater;
            inWater = true;

        }
    }

    /// <summary>
    /// 플레이어가 물밖으로 나갔을때
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "FollowCamera")
        {
            RenderSettings.fogDensity = originColor;
            inWater = false;
        }
    }
}
