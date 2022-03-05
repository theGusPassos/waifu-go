using UnityEngine;

namespace WaifuGO.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        private AudioSource musicPlayer;
        private AudioSource sfxPlayer;

        public AudioClip[]  walkingClips;
        public AudioClip[]  battleClips;
        public AudioClip    startBattleClip;

        public float musicTransitionSpeed;
        public float musicAppearanceSpeed;

        private bool isPlaying = false;
        private bool hasNext = false;
        private int lastMusic;

        private AudioClip nextToPlay;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            musicPlayer = GetComponents<AudioSource>()[0];
            sfxPlayer = GetComponents<AudioSource>()[1];

            StartMusic();
        }

        private void Update()
        {
            if (hasNext)
            {
                if (musicPlayer.volume > 0)
                {
                    musicPlayer.volume -= musicTransitionSpeed * Time.deltaTime;
                }
                else
                {
                    musicPlayer.Stop();
                    musicPlayer.PlayOneShot(nextToPlay);
                    hasNext = false;
                }
            }
            else
            {
                if (isPlaying)
                {
                    if (musicPlayer.volume < 0.75f)
                    {
                        musicPlayer.volume += musicAppearanceSpeed * Time.deltaTime;
                    }
                }
            }
        }

        public void StartMusic()
        {
            PlayClip(walkingClips[Random.Range(0, walkingClips.Length)]);
        }

        public void StartWaifuInteraction()
        {
            sfxPlayer.PlayOneShot(startBattleClip);

            PlayClip(battleClips[Random.Range(0, battleClips.Length)]);
        }

        private void PlayClip(AudioClip clip)
        {
            if (isPlaying)
            {
                nextToPlay = clip;
                hasNext = true;
            }
            else
            {
                isPlaying = true;
                musicPlayer.PlayOneShot(clip);
            }
        }
    }
}
