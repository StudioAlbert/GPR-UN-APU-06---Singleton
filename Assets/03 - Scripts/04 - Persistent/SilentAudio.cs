using UnityEngine;

namespace Chapter4Persistent
{
    /// <summary>
    /// Null object: every method of the service, none of them honoured - but
    /// this one says so. A game that never crashes is a game that can fail in
    /// silence.
    /// A plain C# class on purpose - a destroyed MonoBehaviour compares equal to
    /// null in Unity, which would bring back the very problem this removes.
    /// </summary>
    public class SilentAudio : IAudioService
    {
        public void Play(AudioClip clip)
        {
            if (clip == null) return;       // the same contract as the real one

            Debug.LogWarning("No AudioManager around: " + clip.name + " was not played.");
        }

        public void StopAll()
        {
        }
    }
}
