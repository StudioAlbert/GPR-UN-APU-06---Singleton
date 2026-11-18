/// <summary>
/// The access point, and the only piece of chapter 3 that guarantees anything.
/// Two lines carry the whole promise: the field starts on the null object, and
/// Provide(null) falls back to it. There is no path where Current is null.
/// </summary>
public static class Feedback
{
    private static readonly IFeedbackService None = new NoFeedback();

    private static IFeedbackService current = None;

    public static IFeedbackService Current => current;

    public static void Provide(IFeedbackService service) => current = service ?? None;
}
