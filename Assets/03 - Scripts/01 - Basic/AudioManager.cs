using Demo;
using UnityEngine;

namespace Chapter1Basic
{
    /// <summary>
    /// The textbook MonoBehaviour singleton: a static field, a check on awake,
    /// and the duplicate destroyed after the fact. Unity builds the object, so
    /// the class can no longer forbid a second one.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioClip music;
        [SerializeField] private AudioClip shot;

        private readonly Flash flash = new Flash();

        private AudioSource source;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            source = GetComponent<AudioSource>();
            PlayMusic();
        }

        public void Play(AudioClip clip)
        {
            if (clip == null) return;

            source.PlayOneShot(clip);
            flash.Fire();                       // the sound, confirmed on screen
        }

        public void StopAll() => source.Stop();

        private void Update() => flash.Fade(Time.deltaTime);

        private void OnGUI() => flash.Draw();

        private void PlayMusic()
        {
            if (music == null) return;

            source.clip = music;
            source.loop = true;
            source.Play();
        }
    }
}
