/// <summary>
/// The single place that knows whether a real audio service is available.
/// Callers write Sound.Current.Play(clip) and never test for null.
/// </summary>
public static class Sound
{
    private static readonly IAudioService Silent = new SilentAudio();

    private static IAudioService current = Silent;

    public static IAudioService Current => current;

    public static void Provide(IAudioService service) => current = service ?? Silent;
}
