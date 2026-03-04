using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.Prepare();
        }
    }

    // Update is called once per frame
    public void ReplayVideo()
    {
        videoPlayer.time = 0;
        videoPlayer.Play();
    }
}
