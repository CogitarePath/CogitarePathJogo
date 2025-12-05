using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;


public class CutScenesPlay : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private GameObject video;
    private GameObject rawImage;
    public float waitTime;
    public string nextScene;

    private void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Awake()
    {
        //videoPlayer = GetComponentInChildren<VideoPlayer>();
        video = transform.GetChild(0).gameObject;
        rawImage = transform.GetChild(1).gameObject;

        StartCoroutine(WaitStart());

    }

    void OnVideoEnd(VideoPlayer player)
    {
        StartCoroutine(EndScene());
    }


    IEnumerator EndScene()
    {
        rawImage.SetActive(false);
        video.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(waitTime);
        rawImage.SetActive(true);
        video.SetActive(true);
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }
}
