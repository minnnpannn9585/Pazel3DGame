using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Utilities
{
    public class VideoController : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;

        private void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
        }

        public void PlayVideo(VideoClip clip)
        {
            _videoPlayer.clip = clip;
            _videoPlayer.Play();
            EventHandler.PlayVideo(true);
        }

        public void StopVideo()
        {
            EventHandler.PlayVideo(false);
            _videoPlayer.Stop();
            _videoPlayer.clip = null;
        }
    }
}
