using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Factory : MonoBehaviour
{    
    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오는 함수
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetObject(Item type, Transform spawn = null)
    {
        GameObject result = null;
        switch (type)
        {
            case Item.seaglider:
                //result = slimePool?.GetObject(spawn)?.gameObject;                
                break;
            default:
                result = new GameObject();
                break;
        }

        return result;
    }

    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오면서 위치와 각도를 설정하는 함수
    /// </summary>
    /// <param name="type">생성할 오브젝트의 종류</param>
    /// <param name="position">생성할 위치(월드좌표)</param>
    /// <param name="angle">z축 회전 정도</param>
    /// <returns>생성한 오브젝트</returns>
    public GameObject GetObject(Item type, Vector3 position, float angle = 0.0f)
    {
        GameObject obj = GetObject(type);
        obj.transform.position = position;
        obj.transform.Rotate(angle * Vector3.forward);

        switch (type)
        {        
            default:                
                break;
        }
        
        return obj;
    }
}
