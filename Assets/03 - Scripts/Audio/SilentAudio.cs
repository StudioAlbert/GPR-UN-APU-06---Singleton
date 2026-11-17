using UnityEngine;

/// <summary>
/// Null object: it has every method of the service and honours none of them.
/// A plain C# class on purpose - a destroyed MonoBehaviour compares equal to null
/// in Unity, which would bring back the very problem this removes.
/// It logs, so a service missing by accident still leaves a trace.
/// </summary>
public class SilentAudio : IAudioService
{
    public void Play(AudioClip clip)
    {
        Debug.LogWarning("No AudioManager around: '" + (clip == null ? "null" : clip.name) + "' was not played.");
    }

    public void StopAll()
    {
    }
}
