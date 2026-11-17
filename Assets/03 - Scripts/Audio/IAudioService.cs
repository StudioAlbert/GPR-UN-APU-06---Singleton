using UnityEngine;

/// The contract the callers depend on, instead of the AudioManager class.
public interface IAudioService
{
    void Play(AudioClip clip);

    void StopAll();
}
