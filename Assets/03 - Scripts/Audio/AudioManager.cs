using UnityEngine;

/// <summary>
/// Persistent service: it survives the scene changes, and it registers itself
/// so that nobody has to reach for AudioManager.Instance.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : PersistentSingleton<AudioManager>, IAudioService
{
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip shot;

    private AudioSource source;

    public AudioClip Shot => shot;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;   // a duplicate: base.Awake already condemned it

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
        if (clip != null) source.PlayOneShot(clip);
    }

    public void StopAll() => source.Stop();

    private void PlayMusic()
    {
        if (music == null) return;

        source.clip = music;
        source.loop = true;
        source.Play();
    }
}
