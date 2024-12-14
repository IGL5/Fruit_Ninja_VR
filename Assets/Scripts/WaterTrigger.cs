using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
	public FloatingPlatformManager platformManager;
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            platformManager.SetPlayerFalling(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            platformManager.SetPlayerFalling(false);
        }
    }
}