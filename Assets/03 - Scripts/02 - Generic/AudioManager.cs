using Demo;
using Singletons;
using UnityEngine;

namespace Chapter2Generic
{
    /// <summary>
    /// Same manager, none of the plumbing: Singleton&lt;T&gt; carries it. Swap the
    /// base class for RegulatorSingleton&lt;AudioManager&gt; to watch the newest
    /// instance destroy the older one when the HUD scene is loaded on top.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioClip music;
        [SerializeField] private AudioClip shot;

        private readonly Flash flash = new Flash();

        private AudioSource source;

        protected override void Awake()
        {
            base.Awake();

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
