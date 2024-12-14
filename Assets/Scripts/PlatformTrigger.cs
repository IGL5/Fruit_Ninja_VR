using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public FloatingPlatformManager platformManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            platformManager.SetPlayerOnPlatform(true, gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            platformManager.SetPlayerOnPlatform(false, gameObject);
        }
    }
}
