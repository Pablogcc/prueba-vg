using UnityEngine;

namespace AdventureSurvival.Audio
{
    public class SceneMusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip sceneMusic;
        [SerializeField] private bool playOnStart = true;
        [SerializeField] private bool loop = true;

        private void Start()
        {
            if (playOnStart)
            {
                PlaySceneMusic();
            }
        }

        public void PlaySceneMusic()
        {
            GameAudioManager.Instance.PlayMusic(sceneMusic, loop);
        }
    }
}
