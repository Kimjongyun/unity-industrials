using UnityEngine;

public class Vibration_JY : MonoBehaviour
{
    public ControllerVibration vibration; // 컨트롤러 진동 스크립트
    public CameraShaker_JY cameraShaker; // 카메라 흔들림 스크립트

    void Start()
    {
        // ScenarioManager에서 직접 호출
        // Invoke(nameof(ShakeIt), 4f);
        // Invoke(nameof(Earthquake), 4f);
    }

    // 카메라를 흔들기(ScenarioManager에서 호출)
    public void ShakeIt()
    {
        cameraShaker.TriggerShake(8f, 0.15f); // 8초 동안 0.15 강도로 흔들림
    }

    // 컨트롤러 진동 발생(ScenarioManager에서 호출)
    public void Earthquake()
    {
        vibration.VibrateBoth(1.0f, 1.0f); // 양쪽 컨트롤러 1.0 강도로 1.0초 진동
    }
}