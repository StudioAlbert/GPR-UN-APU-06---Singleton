using Demo;
using Singletons;
using UnityEngine;

namespace Chapter3NullObject
{
    /// <summary>
    /// The manager now registers itself as the audio service, so no caller ever
    /// names this class again. Still scene bound: it dies with its scene, and
    /// Sound falls back to the silent one.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>, IAudioService
    {
        [SerializeField] private AudioClip music;
        [SerializeField] private AudioClip shot;

        private readonly Flash flash = new Flash();

        private AudioSource source;

        protected override void Awake()
        {
            base.Awake();

            source = GetComponent<AudioSource>();
            Sound.Provide(this);
            PlayMusic();
        }

        private void OnDestroy()
        {
            if (ReferenceEquals(Sound.Current, this)) Sound.Provide(null);
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
