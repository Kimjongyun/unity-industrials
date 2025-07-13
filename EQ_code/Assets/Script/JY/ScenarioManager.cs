using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using TMPro; // TextMeshPro를 사용하기 위해 추가해야 할 네임스페이스

public class ScenarioManager : MonoBehaviour
{
    [Header("Video Players")]
    public VideoPlayer mainVideoPlayer; // FirMove01 재생용
    public VideoClip firMove01Clip;
    public VideoPlayer skyboxVideoPlayer; // FirMove02 (스카이박스) 재생용
    public VideoClip firMove02Clip;

    [Header("UI Elements")]
    public GameObject dialoguePanel;   // 대화창 패널 전체
    public TextMeshProUGUI dialogueText; // Text 대신 TextMeshProUGUI로 변경
    public GameObject powerPlugUI;     // 전원 코드 뽑기 안내 UI (추후 사용)
    public GameObject nextStageArrow1; // 첫 번째 화살표 UI (추후 사용)
    public GameObject nextStageArrow2; // 두 번째 화살표 UI (추후 사용)
    public GameObject phoneScreenObject; // NEW: 핸드폰의 재난 문자 화면 Plane 오브젝트

    [Header("Interactables")]
    public GameObject powerPlugObject; // power plug 오브젝트 (추후 사용)
    public GameObject cushionObject;   // cushion 오브젝트 (추후 사용)

    [Header("Effects")]
    public Vibration_JY vibrationManager; // Vibration_JY 스크립트 연결

    // 시나리오 진행 상태 변수 (추후 사용)
    private bool powerPlugPulled = false;
    private bool cushionWorn = false;

    void Start()
    {
        // 초기 설정: 모든 UI 숨기기
        dialoguePanel.SetActive(false);
        if (powerPlugUI != null) powerPlugUI.SetActive(false);
        if (nextStageArrow1 != null) nextStageArrow1.SetActive(false);
        if (nextStageArrow2 != null) nextStageArrow2.SetActive(false);
        if (phoneScreenObject != null) phoneScreenObject.SetActive(false); // NEW: 폰 화면도 초기에는 숨김

        // 1단계 시작: 영상 재생 준비 및 시나리오 시작
        StartCoroutine(StartScenario1());
    }

    IEnumerator StartScenario1()
    {
        // 1. FirMove01 영상 재생 시작 (멈춤 없이 계속 재생)
        if (mainVideoPlayer != null && firMove01Clip != null)
        {
            mainVideoPlayer.clip = firMove01Clip; // FirMove01 클립 할당
            mainVideoPlayer.Play(); // 영상 재생 시작
            Debug.Log("FirMove01 video started playing immediately.");
        }
        else
        {
            Debug.LogError("Main Video Player or FirMove01 Clip is not assigned in ScenarioManager!");
        }

        // 2. 진동 및 카메라 흔들림 시작
        if (vibrationManager != null)
        {
            vibrationManager.ShakeIt(); // 카메라 흔들림 시작 (Vibration_JY 스크립트의 함수 호출)
            vibrationManager.Earthquake(); // 컨트롤러 진동 시작 (Vibration_JY 스크립트의 함수 호출)
            Debug.Log("Vibration and camera shake started.");
        }
        else
        {
            Debug.LogError("Vibration Manager is not assigned in ScenarioManager!");
        }

        // 3. 진동 시작 후 5초 뒤 뉴스 및 핸드폰 화면 활성화
        yield return new WaitForSeconds(4f); // 진동 시작 후 5초 대기 (이 시간 동안 영상은 계속 재생됩니다)

        // NEW: 5초 대기 후 영상 정지 (멈춘 프레임 유지가 아닌, 아예 정지시키고 초기 프레임으로 돌아갑니다.)
        if (mainVideoPlayer != null)
        {
            mainVideoPlayer.Stop(); 
            Debug.Log("FirMove01 video stopped after 5 seconds of playing.");
        }

        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = "긴급 재난 방송입니다. 현재 지진이 발생하고 있습니다. 안전한 곳으로 대피하십시오.";
            Debug.Log("Disaster broadcast started (News text).");
        }
        if (phoneScreenObject != null) // 핸드폰 화면 활성화
        {
            phoneScreenObject.SetActive(true);
            Debug.Log("Phone disaster message screen activated.");
        }
        else
        {
            Debug.LogError("Phone Screen Object is not assigned in ScenarioManager!");
        }

        // 4. 재난 방송이 충분히 노출될 시간
        yield return new WaitForSeconds(16f); 

        // 5. 전원 코드 뽑기 안내 텍스트
        if (dialogueText != null)
        {
            dialogueText.text = "주변의 전원 코드를 뽑아 2차 피해를 예방하세요.";
            Debug.Log("Prompt for power plug action.");
        }
        // powerPlugUI는 아직 만들지 않았으므로, 이 부분은 다음 단계에서 활성화됩니다.
        // if (powerPlugUI != null) powerPlugUI.SetActive(true);
    }

    // --- 사용자의 '전원 코드 뽑기' 상호작용 후 호출될 메서드 ---
    public void OnPowerPlugPulled()
    {
        if (!powerPlugPulled)
        {
            powerPlugPulled = true;
            Debug.Log("Power plug pulled action detected.");
            // if (powerPlugUI != null) powerPlugUI.SetActive(false); // UI가 있다면 비활성화
            if (dialogueText != null) 
            {
                dialogueText.text = "전원 코드를 뽑았습니다. 이제 머리를 보호할 물건을 찾으세요.";
            }
            if (phoneScreenObject != null) phoneScreenObject.SetActive(false); // 전화 화면도 끄기
            Debug.Log("Prompt for cushion action.");
        }
    }

    // --- 사용자의 '쿠션 사용' 상호작용 후 호출될 메서드 ---
    public void OnCushionWorn()
    {
        if (!cushionWorn)
        {
            cushionWorn = true;
            Debug.Log("Cushion worn action detected.");
            if (dialogueText != null) 
            {
                dialogueText.text = "쿠션으로 머리를 보호했습니다. 이제 안전한 장소로 이동하세요.";
            }
            if (nextStageArrow1 != null) nextStageArrow1.SetActive(true); // 다음 단계 화살표 활성화
            if (dialoguePanel != null) dialoguePanel.SetActive(true); // 대화 패널 다시 활성화 (화살표와 함께)
        }
    }

    // --- 사용자의 '첫 번째 화살표' 클릭 상호작용 후 호출될 메서드 ---
    public void OnFirstArrowClicked()
    {
        Debug.Log("First arrow clicked. Resuming main video (FirMove01).");
        if (nextStageArrow1 != null) nextStageArrow1.SetActive(false); // 화살표 비활성화
        if (mainVideoPlayer != null)
        {
            // 이전에 Stop() 했으므로, 다시 Play()를 호출하여 재생 시작
            mainVideoPlayer.Play(); // FirMove01 영상 다시 재생
        }
        if (dialoguePanel != null) dialoguePanel.SetActive(false); // 대화창 비활성화

        StartCoroutine(WaitForMainVideoEnd()); // FirMove01 영상 끝날 때까지 대기
    }

    // --- FirMove01 영상이 끝날 때까지 대기하고 다음 단계 진행 ---
    IEnumerator WaitForMainVideoEnd()
    {
        if (mainVideoPlayer != null && mainVideoPlayer.clip != null)
        {
            yield return new WaitForSeconds((float)mainVideoPlayer.clip.length - (float)mainVideoPlayer.time); // 남은 시간만큼 대기
            mainVideoPlayer.Stop(); // 영상 정지
            Debug.Log("Main video (FirMove01) ended. Activating second arrow.");
        }
        else
        {
            Debug.LogError("Main Video Player or clip is null when waiting for video end.");
        }
        if (nextStageArrow2 != null) nextStageArrow2.SetActive(true); // 두 번째 화살표 활성화
    }

    // --- 사용자의 '두 번째 화살표' 클릭 상호작용 후 호출될 메서드 ---
    public void OnSecondArrowClicked()
    {
        Debug.Log("Second arrow clicked. Starting FirMove02 skybox video.");
        if (nextStageArrow2 != null) nextStageArrow2.SetActive(false); // 두 번째 화살표 비활성화
        if (skyboxVideoPlayer != null && firMove02Clip != null)
        {
            skyboxVideoPlayer.clip = firMove02Clip; // FirMove02 클립 할당
            skyboxVideoPlayer.Play(); // FirMove02 영상 재생
        }
        else
        {
            Debug.LogError("Skybox Video Player or FirMove02 Clip is not assigned in ScenarioManager!");
        }
        if (dialoguePanel != null) dialoguePanel.SetActive(false); // 대화창이 혹시 남아있다면 비활성화
    }
}