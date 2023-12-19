using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    Color originColor;
    Color underWater;


    void Awake()
    {
        originColor = RenderSettings.fogColor;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" && this.GetComponent<Collider>().bounds.Contains(other.bounds.min)
            && this.GetComponent<Collider>().bounds.Contains(other.bounds.max))
        {
            Debug.Log("플레이어가 물 안에 머무는중");
            RenderSettings.fogColor = underWater;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("플레이어가 물 밖으로 나감");
            RenderSettings.fogColor = originColor;
        }
    }

    private void SmoothColorChange()
    {
        //if (RenderSettings.fogColor != underWater)
        //{
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, underWater, 0.1f);
        //}

    }
}
