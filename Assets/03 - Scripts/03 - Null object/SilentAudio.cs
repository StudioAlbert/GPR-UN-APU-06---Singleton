using UnityEngine;

namespace Chapter3NullObject
{
    /// <summary>
    /// Null object: every method of the service, none of them honoured.
    /// Nothing is logged here: that is what the last step of chapter 3 adds.
    /// A plain C# class on purpose - a destroyed MonoBehaviour compares equal to
    /// null in Unity, which would bring back the very problem this removes.
    /// </summary>
    public class SilentAudio : IAudioService
    {
        public void Play(AudioClip clip)
        {
        }

        public void StopAll()
        {
        }
    }
}
