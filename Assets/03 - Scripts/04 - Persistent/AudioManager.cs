using Demo;
using Singletons;
using UnityEngine;

namespace Chapter4Persistent
{
    /// <summary>
    /// The end state of the session: persistent, so it survives the scene
    /// change, and registered, so no caller can ever meet a null. The two last
    /// chapters are the two halves of the same answer.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : PersistentSingleton<AudioManager>, IAudioService
    {
        [SerializeField] private AudioClip music;
        [SerializeField] private AudioClip shot;

        private readonly Flash flash = new Flash();

        private AudioSource source;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;       // a duplicate: base.Awake already condemned it

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
