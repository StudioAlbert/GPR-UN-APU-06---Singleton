using UnityEngine;

namespace Chapter3NullObject
{
    /// <summary>
    /// The contract, and nothing more. On its own it guarantees nothing: all it
    /// does is make two implementations interchangeable.
    /// </summary>
    public interface IAudioService
    {
        void Play(AudioClip clip);

        void StopAll();
    }
}
