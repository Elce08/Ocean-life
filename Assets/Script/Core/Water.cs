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
            Debug.Log("�÷��̾ �� �ȿ� �ӹ�����");
            RenderSettings.fogColor = underWater;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("�÷��̾ �� ������ ����");
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
