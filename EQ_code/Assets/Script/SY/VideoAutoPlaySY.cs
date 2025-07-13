using UnityEngine;
using UnityEngine.Video;

public class VideoAutoPlaySY : MonoBehaviour
{
    private VideoPlayer video;

    void Start()
    {
        video = GetComponent<VideoPlayer>();
        StartCoroutine(PlayAndStopVideo());
    }

    private System.Collections.IEnumerator PlayAndStopVideo() // 코루틴(Coroutine)
    {
        yield return new WaitForSeconds(5f); // 5초 대기 후 재생
        video.Play();
        yield return new WaitForSeconds(13f); // 재생 후 13초 뒤 정지
        video.Stop(); // 또는 video.Pause();


        GetComponent<Renderer>().material.color = Color.black;//화면 검게 만들기 
    }
}
