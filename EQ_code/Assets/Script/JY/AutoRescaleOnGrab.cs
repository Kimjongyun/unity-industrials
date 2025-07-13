using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AutoRescaleOnGrab : MonoBehaviour
{
    // 잡아당길 때 적용할 최종 로컬 스케일입니다. 인스펙터에서 원하는 줄어든 크기로 설정하세요.
    // 현재 로그에서 (0.01, 0.01, 0.01)로 강제되는 경향이 있으니,
    // 만약 이보다 더 작게 만들고 싶다면 (0.005f, 0.005f, 0.005f) 등으로 설정해 보세요.
    // 만약 잡았을 때 (0.3, 0.3, 0.3)이 되기를 원한다면, 이 값을 (0.3f, 0.3f, 0.3f)로 설정하고,
    // LateUpdate에서 강제 적용되는지 확인해야 합니다.
    public Vector3 grabbedTargetLocalScale = new Vector3(1f, 1f, 1f); // 예시: 더 작은 값으로 설정

    private Vector3 originalLocalScale;
    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false; // 오브젝트가 잡혔는지 추적하는 변수

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable)
        {
            grabInteractable.trackPosition = true;
            grabInteractable.trackRotation = true;
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    private void Start()
    {
        originalLocalScale = transform.localScale;
        Debug.Log("Start() - Original Local Scale saved as: " + originalLocalScale);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true; // 잡힘 상태로 설정
        Debug.Log("Grabbed! Attempting to rescale object.");

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Attach Transform의 스케일을 (1,1,1)로 강제 설정 (이전과 동일)
        if (grabInteractable.attachTransform != null)
        {
            grabInteractable.attachTransform.localScale = Vector3.one;
            Debug.Log("Attach Transform Scale reset to: " + grabInteractable.attachTransform.localScale);
        }

        // OnGrab 시점에도 일단 스케일 적용 시도
        transform.localScale = grabbedTargetLocalScale;

        Debug.Log("Object Name: " + gameObject.name);
        Debug.Log("OnGrab - After initial Rescale Attempt - Local Scale: " + transform.localScale);
        Debug.Log("OnGrab - After initial Rescale Attempt - World Scale: " + transform.lossyScale);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false; // 잡힘 상태 해제
        Debug.Log("Released! Restoring original scale.");

        transform.localScale = originalLocalScale;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    // LateUpdate에서 매 프레임마다 스케일을 강제로 적용합니다.
    private void LateUpdate()
    {
        if (isGrabbed)
        {
            // 현재 스케일이 목표 스케일과 다르면 강제로 설정
            if (transform.localScale != grabbedTargetLocalScale)
            {
                transform.localScale = grabbedTargetLocalScale;
                Debug.Log("LateUpdate: Forcing scale to " + grabbedTargetLocalScale);
            }

            // Attach Transform의 스케일도 매 프레임 확인 및 강제 (필요하다면)
            if (grabInteractable.attachTransform != null && grabInteractable.attachTransform.localScale != Vector3.one)
            {
                grabInteractable.attachTransform.localScale = Vector3.one;
                Debug.Log("LateUpdate: Forcing Attach Transform scale to " + Vector3.one);
            }
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
}