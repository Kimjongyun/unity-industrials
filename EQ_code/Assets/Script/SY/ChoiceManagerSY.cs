using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.Video;

public class ChoiceManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;          // VideoPlayer 연결
    public VideoClip nextVideoClip;          // 바꿔줄 '책상 아래' 영상

    // 선택지 버튼
    public Button optionA;
    public Button optionB;
    public Button optionC;

    // "다시 선택하기" 버튼
    public Button retryButton;

    // "NEXT" 버튼
    public Button nextButton;
    private bool isNextStage = false; // NEXT를 한 번 눌렀는지 여부

    // 대화 텍스트 
    public Text dialogueText;

    // UI 그룹 오브젝트들
    public GameObject dialoguePanel;   // 대화창 패널 전체
    public GameObject optionPanel;     // 선택지 버튼 그룹 전체

    // 이미 선택된 버튼 추적용
    private bool aSelected = false;
    private bool cSelected = false;

    // 현재 다시 선택 대기 중인지 체크
    private bool isWaitForRetry = false;

    void Start()
    {
        // 처음에 UI 보이도록 + Retry 버튼 숨기기
        dialoguePanel.SetActive(true);
        optionPanel.SetActive(true);
        retryButton.gameObject.SetActive(false);

        // 숨는 곳 선택 첫 대사 출력
        dialogueText.text = "머리를 보호할 물건은 확보했습니다.이제 몸을 숨길 수 있는 안전한 \n장소를 선택해야 합니다.숨을 장소를 선택해 주세요.";

        // 각 버튼 클릭 시 SelectOption 실행 (Coroutine 사용/ 람다식)
        optionA.onClick.AddListener(() => StartCoroutine(SelectOption("A", optionA)));
        optionB.onClick.AddListener(() => StartCoroutine(SelectOption("B", optionB)));
        optionC.onClick.AddListener(() => StartCoroutine(SelectOption("C", optionC)));


    }

    // 선택지 버튼 클릭 시 0.5초간 모든 UI 잠시 숨기기 > 뭔가 '인식중...' 느낌 표현 > 필요 없으면 삭제 예정 
    IEnumerator SelectOption(string option, Button selectedButton)
    {
        // 만약 현재 '다시 선택하기' 대기 상태면, 선택 막기
        if (isWaitForRetry) yield break;

        // 선택된 버튼을 즉시 비활성화하고 투명도 낮추기
        selectedButton.interactable = false;
        Color faded = selectedButton.image.color;
        faded.a = 0.3f;
        selectedButton.image.color = faded;

        // UI 일시적으로 숨기기
        dialoguePanel.SetActive(false);
        optionPanel.SetActive(false);
        retryButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (option == "B")
        {
            // SceneManager.LoadScene("SceneCh01"); // 씬이동 > 말고 한 씬으로 제작하기

            dialoguePanel.SetActive(true);
            optionPanel.SetActive(false);
            retryButton.gameObject.SetActive(false); // 다시선택 숨기기
            nextButton.gameObject.SetActive(true);   // NEXT 버튼 표시
            dialogueText.text = "정답이야! 이제 실제로 튼튼한 책상 밑으로 들어보자.";

            // NEXT 버튼 클릭 시 탁자아래 연결
            nextButton.onClick.RemoveAllListeners(); // 전에 쌓인 이벤트 리스너 제거
            nextButton.onClick.AddListener(OnNextClicked); // 버튼 클릭 시 실행할 동작 새로 지정
        }
        else if ((option == "A" && !aSelected) || (option == "C" && !cSelected))
        {
            // 상태 변경: 다른 선택지 막기
            isWaitForRetry = true;

            // 대화창과 Retry 버튼 다시 표시
            dialoguePanel.SetActive(true);
            optionPanel.SetActive(true);
            retryButton.gameObject.SetActive(true);
            dialogueText.text = "몸을 숨기기에 너무 부실한 것 같다. 다른 장소를 찾아보자.";

            if (option == "A") aSelected = true;
            else if (option == "C") cSelected = true;

            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(() => Retry());
        }
    }

    // "다시 선택하기" 버튼 클릭 시 처리
    void Retry()
    {
        // 옵션 버튼 다시 선택 가능 상태로 전환
        isWaitForRetry = false;

        retryButton.gameObject.SetActive(false);
        optionPanel.SetActive(true);
        dialogueText.text = "다른 선택지를 골라봐.";
    }

    void OnNextClicked()
    {

        if (!isNextStage)
        {
            // 텍스트만 변경
            dialogueText.text = "진동이 잦아질 때까지 기다립니다.\n진동이 잦아들면 탁자에서 나와 안전을 확보합니다.";
            isNextStage = true; // 다음 클릭에서 씬 전환하도록 표시

            // 영상 전환 > 책상 아래
            if (videoPlayer != null && nextVideoClip != null)
            {
                videoPlayer.Stop();
                videoPlayer.clip = nextVideoClip;
                videoPlayer.Play();
            }

        }
        else
        {
            // 두 번째 NEXT 클릭시, 씬 이동
            SceneManager.LoadScene("SceneIntro");
        }

    }


}
