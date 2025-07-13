using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Cushion : MonoBehaviour
{
    public float triggerHeight = 0.3f;
    public float requiredHoldTime = 3f; // 최소 유지 시간
    public Transform baseHeightReference;

    private XRGrabInteractable grabInteractable;
    private Transform interactorTransform;

    private float timer = 0f;
    private bool sceneChanged = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener((args) =>
        {
            if (args.interactorObject is IXRSelectInteractor interactor)
            {
                interactorTransform = interactor.transform;
                timer = 0f; // 리셋
            }
        });

        grabInteractable.selectExited.AddListener((args) =>
        {
            interactorTransform = null;
            timer = 0f;
        });
    }

    void Update()
    {
        if (sceneChanged || interactorTransform == null)
            return;

        float lifted = interactorTransform.position.y - baseHeightReference.position.y;

        if (lifted >= triggerHeight)
        {
            timer += Time.deltaTime;

            if (timer >= requiredHoldTime)
            {
                sceneChanged = true;
                SceneManager.LoadScene("SceneCh02");
            }
        }
        else
        {
            // 기준보다 내려가면 타이머 리셋
            timer = 0f;
        }
    }
}
