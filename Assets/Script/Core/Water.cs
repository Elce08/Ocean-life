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
    /// �÷��̾ ���ӿ� ��������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FollowCamera")  // ������ �ݶ��̴���
        {
            RenderSettings.fogDensity = underWater; // Densitiy�� ����
            inWater = true;
        }
    }

    /// <summary>
    /// �÷��̾ �������� ��������
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "FollowCamera")  // ������ �ݶ��̴��� ������
        {
            RenderSettings.fogDensity = originColor;    // Densitiy�� ���󺹱�
            inWater = false;
        }
    }
}
