namespace Chapter4Persistent
{
    /// <summary>
    /// The access point, and the only piece of chapter 3 that guarantees
    /// anything. Two lines carry the whole promise: the field starts on the null
    /// object, and Provide(null) falls back to it. Current is never null.
    /// Named Sound and not Audio: UnityEngine.Audio is a namespace.
    /// </summary>
    public static class Sound
    {
        private static readonly IAudioService Silent = new SilentAudio();

        private static IAudioService current = Silent;

        public static IAudioService Current => current;

        public static void Provide(IAudioService service) => current = service ?? Silent;
    }
}
