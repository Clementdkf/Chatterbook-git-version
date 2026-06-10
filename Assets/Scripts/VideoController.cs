using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    // Start is called before the first frame update
    void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
    }
    private void OnVideoPrepared(VideoPlayer source)
    {
        //videoPlayer.StepForward();
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        videoPlayer.Pause();
    }

    public void ReplayVideo()
    {
        videoPlayer.time = 0;
        videoPlayer.Play();
    }
}
