using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Factory
{
    private readonly ItemManager itemManager;

    /// <summary>
    /// 오브젝트를 풀에서 하나 가져오는 함수
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetObject(Item type)
    {
        GameObject result = null;
        // 변경 예정
        switch (type)
        {
            case Item.seaglider:
                //result = objectPool?.GetObject()?.gameObject;
                itemManager.Add(Item.seaglider);
                break;

            case Item.airtank:
                itemManager.Add(Item.airtank);
                break;

            case Item.head:
                itemManager.Add(Item.head);
                break;

            case Item.body:
                itemManager.Add(Item.body);
                break;

            case Item.titanium:
                itemManager.Add(Item.titanium);
                break;

            case Item.copper:
                itemManager.Add(Item.copper);
                break;

            case Item.coal:
                itemManager.Add(Item.coal);
                break;

            case Item.quartz:
                itemManager.Add(Item.quartz);
                break;

            case Item.plastic:
                itemManager.Add(Item.plastic);
                break;

            case Item.glass:
                itemManager.Add(Item.glass);
                break;

            case Item.coppercable:
                itemManager.Add(Item.coppercable);
                break;

            case Item.water:
                itemManager.Add(Item.water);
                break;

            case Item.fish1:
                itemManager.Add(Item.fish1);
                break;

            case Item.fish2:
                itemManager.Add(Item.fish2);
                break;

            case Item.fish3:
                itemManager.Add(Item.fish3);
                break;

            case Item.fish4:
                itemManager.Add(Item.fish4);
                break;

            case Item.cookedFish1:
                itemManager.Add(Item.cookedFish1);
                break;

            case Item.cookedFish2:
                itemManager.Add(Item.cookedFish2);
                break;

            case Item.cookedFish3:
                itemManager.Add(Item.cookedFish3);
                break;

            case Item.cookedFish4:
                itemManager.Add(Item.cookedFish4);
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
