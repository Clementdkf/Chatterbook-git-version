using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
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
        videoPlayer.StepForward();
    }

    // Update is called once per frame
    public void ReplayVideo()
    {
        videoPlayer.time = 0;
        videoPlayer.Play();
    }
}
