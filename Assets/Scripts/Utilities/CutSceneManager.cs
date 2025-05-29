using UnityEngine;
using UnityEngine.Playables;

namespace Utilities
{
    public class CutSceneManager : MonoBehaviour
    {
        public void PlayCutScene(PlayableDirector director)
        {
            director.gameObject.SetActive(true);
            director.Play();
        }
    }
}
