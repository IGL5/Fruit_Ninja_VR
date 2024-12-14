using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BigRookGames.Build;
using UnityEngine.XR.Interaction.Toolkit;

public class FloatingPlatformManager : MonoBehaviour
{
    public List<GameObject> platforms;
    public GameObject player;

    public int centralPlatformIndex = 0;
    public float minInterval = 15f;
    public float maxInterval = 20f;

    private GameObject activePlatform;
    private GameObject secondaryPlatform;

    private Dictionary<GameObject, bool> platformStatus = new Dictionary<GameObject, bool>();
	private bool playerFalling = false;
	private Coroutine rotationCoroutine = null;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (var platform in platforms)
        {
            platform.SetActive(false);
            platformStatus[platform] = false;
        }

        ActivatePlatform(centralPlatformIndex);
        activePlatform = platforms[centralPlatformIndex];
        secondaryPlatform = null;
        platformStatus[activePlatform] = true;

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        rotationCoroutine = StartCoroutine(PlatformRotationRoutine());
    }

    private void reinit()
    {
        if (secondaryPlatform != null)
        {
            secondaryPlatform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(4);
            secondaryPlatform.SetActive(false);
        }
        if (activePlatform != null)
        {
            activePlatform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(4);
            activePlatform.SetActive(false);
        }

        if (player != null)
        {
            player.transform.position = new Vector3(0, 2.5f, 0);
        }

        Initialize();
    }

    private IEnumerator PlatformRotationRoutine()
    {
        while (true)
        {
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            int nextIndex1, nextIndex2;

            do
            {
                nextIndex1 = Random.Range(0, platforms.Count);
            } while (platforms[nextIndex1] == activePlatform);

            do
            {
                nextIndex2 = Random.Range(0, platforms.Count);
            } while (nextIndex2 == nextIndex1 || platforms[nextIndex2] == activePlatform);

            GameObject nextPlatform1 = platforms[nextIndex1];
            GameObject nextPlatform2 = platforms[nextIndex2];

            ActivatePlatform(nextIndex1);
            ActivatePlatform(nextIndex2);

            secondaryPlatform = nextPlatform2;

            yield return new WaitForSeconds(2f);

            StartCoroutine(DestroyPlatform(activePlatform));

            yield return new WaitForSeconds(4.5f);
            StartCoroutine(DeactivateUnusedPlatform());

            activePlatform = nextPlatform1;
        }
    }

    private void ActivatePlatform(int index)
    {
        if (index < 0 || index >= platforms.Count) return;

        GameObject platform = platforms[index];
        platform.SetActive(true);
        platform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(0);
    }

    private IEnumerator DestroyPlatform(GameObject platform)
    {
        if (platform == null) yield break;

        platform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(1);
        SendHapticFeedback(0.2f, 0.5f, platform);
        yield return new WaitForSeconds(1f);

        platform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(2);
        SendHapticFeedback(0.5f, 0.5f, platform);
        yield return new WaitForSeconds(1f);

        platform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(3);
        SendHapticFeedback(1f, 1f, platform);
        yield return new WaitForSeconds(1f);

        platform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(4);
        yield return new WaitForSeconds(1f);
        platform.SetActive(false);
    }

    private IEnumerator DeactivateUnusedPlatform()
    {
        if (secondaryPlatform == null) yield break;

        if (!platformStatus[secondaryPlatform])
        {
            secondaryPlatform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(4);
            yield return new WaitForSeconds(0.5f);
            secondaryPlatform.SetActive(false);
        }
        else
        {
            if (activePlatform != null)
            {
                activePlatform.GetComponent<BasicWoodWallController>()?.PlayDamageAnimation(4);
                yield return new WaitForSeconds(0.5f);
                activePlatform.SetActive(false);
            }

            activePlatform = secondaryPlatform;
        }

        secondaryPlatform = null;
    }

    private void SendHapticFeedback(float amplitude, float duration, GameObject platform)
    {
        if (platformStatus[platform])
        {
            XRDirectInteractor[] interactors = Object.FindObjectsByType<XRDirectInteractor>(FindObjectsSortMode.None);

            foreach (XRDirectInteractor interactor in interactors)
            {
                interactor.SendHapticImpulse(amplitude, duration);
            }
        }
    }

    public void SetPlayerOnPlatform(bool isOnPlatform, GameObject platform)
    {
        if (platformStatus.ContainsKey(platform))
        {
            platformStatus[platform] = isOnPlatform;
        }
    }
	
	public void SetPlayerFalling(bool isFalling)
    {
        playerFalling = isFalling;
		
		
		
		if (isFalling) {
			reinit();
		}
    }
}