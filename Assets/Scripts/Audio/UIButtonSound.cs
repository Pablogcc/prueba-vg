using UnityEngine;

namespace AdventureSurvival.Audio
{
    public class UIButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioClip clickSound;
        [Range(0f, 1f)]
        [SerializeField] private float volume = 1f;

        public void PlayClickSound()
        {
            GameAudioManager.Instance.PlaySfx(clickSound, volume);
        }

        private void OnValidate()
        {
            volume = Mathf.Clamp01(volume);
        }
    }
}
