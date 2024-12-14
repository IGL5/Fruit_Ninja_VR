using UnityEngine;
using System.Collections;

public class CircularMovement : MonoBehaviour
{
    public Transform centerPoint;
    public float baseSpeed = 0.25f;
    public bool isShark = false;

    public float verticalAmplitude = 0.5f;
    public float verticalSpeed = 2f;
    public float maxWaterHeight = 0.3f;

    private float currentSpeed;
    private float radius;
    private float currentAngle;
    private float initial_y;
    private Coroutine speedCoroutine;

    void Start()
    {
        if (centerPoint == null)
        {
            Debug.LogError("Debes asignar un centro en el inspector.");
            return;
        }

        Vector3 offset = transform.position - centerPoint.position;
        radius = offset.magnitude;
        initial_y = transform.position.y;

        currentAngle = Mathf.Atan2(offset.z, offset.x);

        currentSpeed = baseSpeed;

        if (isShark)
        {
            speedCoroutine = StartCoroutine(AdjustSpeedRoutine());
        }
    }

    void Update()
    {
        if (centerPoint == null) return;

        currentAngle += currentSpeed * Time.deltaTime;

        float x = Mathf.Cos(currentAngle) * radius;
        float z = Mathf.Sin(currentAngle) * radius;

        if (isShark)
        {
            float targetHeight = Mathf.Min(centerPoint.position.y, maxWaterHeight);
            initial_y = Mathf.Lerp(initial_y, targetHeight, Time.deltaTime * 2f);
        }

        float y = initial_y + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;

        Vector3 newPosition = Vector3.Lerp(transform.position, centerPoint.position + new Vector3(x, y, z), Time.deltaTime * 2f);
        newPosition.y = y;

        Vector3 direction = new Vector3(-Mathf.Sin(currentAngle), 0, Mathf.Cos(currentAngle));
        if (baseSpeed < 0)
        {
            direction = -direction;
        }

        transform.position = newPosition;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        if (isShark)
        {
            float yawOffset = Mathf.Sin(Time.time * currentSpeed * 20f) * 8f;
            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y + yawOffset, targetRotation.eulerAngles.z);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
    }

    private IEnumerator AdjustSpeedRoutine()
    {
        while (true)
        {
            currentSpeed = baseSpeed * Random.Range(1.5f, 3f);
            yield return new WaitForSeconds(Random.Range(1f, 2.5f));

            currentSpeed = baseSpeed;
            yield return new WaitForSeconds(Random.Range(2f, 4.5f));
        }
    }

    void OnDisable()
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
        }
    }
}
