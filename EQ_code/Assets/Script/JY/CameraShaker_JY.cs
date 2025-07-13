using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class CameraShaker_JY : MonoBehaviour
{
    public float shakeDuration = 10f;
    public float shakeAngle = 10f;

    private Quaternion initRot;
    private float remainShakeTime;

    private void Start()
    {
        initRot = transform.localRotation;
    }

    private void Update()
    {
        if (remainShakeTime > 0)
        {
            Quaternion randomRot = Quaternion.Euler(
                Random.Range(-shakeAngle, shakeAngle),
                Random.Range(-shakeAngle, shakeAngle),
                0f
            );

            transform.localRotation = initRot * randomRot;

            remainShakeTime -= Time.deltaTime;

            if (remainShakeTime <= 0f)
            {
                transform.localRotation = initRot;
            }
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        // duration = -1f;
        // magnitude = -1f;

        if (duration > 0) shakeDuration = duration;
        if (magnitude > 0) shakeAngle = magnitude;

        remainShakeTime = shakeDuration;
    }
}
