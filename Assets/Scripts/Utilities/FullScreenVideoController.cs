using System;
using UnityEngine;
using UnityEngine.Video;

namespace Utilities
{
    public class FullScreenVideoController : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;
        public GameObject audioManager;

        private void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
            _videoPlayer.loopPointReached += StopVideo;
        }

        public void PlayFullScreenVideo(VideoClip clip)
        {
            _videoPlayer.clip = clip;
            _videoPlayer.Play();
            EventHandler.HandleFullScreenVideo(true);
            Time.timeScale = 0;
            EventHandler.AllowInputControl(false);
            audioManager.SetActive(false);
        }
        
        public void StopVideo(VideoPlayer videoPlayer)
        {
            EventHandler.HandleFullScreenVideo(false);
            videoPlayer.Stop();
            videoPlayer.clip = null;
            Time.timeScale = 1;
            EventHandler.AllowInputControl(true);
            audioManager.SetActive(true);
        }
    }
}