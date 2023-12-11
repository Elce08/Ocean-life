using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public bool canBreathe = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Water")) canBreathe = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water")) canBreathe = true;
    }
}
