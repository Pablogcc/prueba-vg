using UnityEngine;

namespace AdventureSurvival.Audio
{
    public class GameAudioManager : MonoBehaviour
    {
        private static GameAudioManager instance;

        [Header("Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Startup Music")]
        [SerializeField] private AudioClip startupMusic;
        [SerializeField] private bool playStartupMusic = true;

        [Header("Volumes")]
        [Range(0f, 1f)]
        [SerializeField] private float musicVolume = 0.7f;
        [Range(0f, 1f)]
        [SerializeField] private float sfxVolume = 1f;

        [Header("Persistence")]
        [SerializeField] private bool keepBetweenScenes = true;

        public static GameAudioManager Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = FindObjectOfType<GameAudioManager>();

                if (instance != null)
                {
                    return instance;
                }

                GameObject managerObject = new GameObject(nameof(GameAudioManager));
                instance = managerObject.AddComponent<GameAudioManager>();
                return instance;
            }
        }

        public float MusicVolume => musicVolume;
        public float SfxVolume => sfxVolume;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            EnsureAudioSources();
            ApplyVolumes();

            if (keepBetweenScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            if (playStartupMusic && startupMusic != null)
            {
                PlayMusic(startupMusic);
            }
        }

        public void PlayMusic(AudioClip musicClip, bool loop = true)
        {
            if (musicClip == null)
            {
                return;
            }

            EnsureAudioSources();

            if (musicSource.clip == musicClip && musicSource.isPlaying)
            {
                return;
            }

            musicSource.clip = musicClip;
            musicSource.loop = loop;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }

        public void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }

        public void PlaySfx(AudioClip sfxClip)
        {
            PlaySfx(sfxClip, 1f);
        }

        public void PlaySfx(AudioClip sfxClip, float volumeMultiplier)
        {
            if (sfxClip == null)
            {
                return;
            }

            EnsureAudioSources();
            sfxSource.PlayOneShot(sfxClip, sfxVolume * Mathf.Clamp01(volumeMultiplier));
        }

        public void PlaySfxAtPosition(AudioClip sfxClip, Vector3 position, float volumeMultiplier = 1f)
        {
            if (sfxClip == null)
            {
                return;
            }

            AudioSource.PlayClipAtPoint(sfxClip, position, sfxVolume * Mathf.Clamp01(volumeMultiplier));
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            ApplyVolumes();
        }

        public void SetSfxVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            ApplyVolumes();
        }

        public void LoadSceneMusic(AudioClip sceneMusic)
        {
            PlayMusic(sceneMusic);
        }

        private void EnsureAudioSources()
        {
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.playOnAwake = false;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }
        }

        private void ApplyVolumes()
        {
            if (musicSource != null)
            {
                musicSource.volume = musicVolume;
            }

            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
            }
        }

        private void OnValidate()
        {
            musicVolume = Mathf.Clamp01(musicVolume);
            sfxVolume = Mathf.Clamp01(sfxVolume);
        }
    }
}
