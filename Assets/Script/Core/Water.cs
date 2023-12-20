using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    float originColor = 0.05f;
    float underWater = 0.10f;
    public bool inWater;


    void Awake()
    {
        originColor = RenderSettings.fogDensity;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" && this.GetComponent<Collider>().bounds.Contains(other.bounds.min)
            && this.GetComponent<Collider>().bounds.Contains(other.bounds.max))
        {
            RenderSettings.fogDensity = underWater;
            inWater = true;
            
        }
        else
        {
            RenderSettings.fogDensity = originColor;
            inWater = false;
        }
    }

/*    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("플레이어가 물 밖으로 나감");
            RenderSettings.fogColor = originColor;
        }
    }
    
    private void SmoothColorChange()
    {
        if (RenderSettings.fogColor != underWater)
        {
          RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, underWater, 0.1f);
        }
    
    }*/

}
