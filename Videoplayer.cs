using UnityEngine;
using UnityEngine.Video;
public class Videoplayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }

}
