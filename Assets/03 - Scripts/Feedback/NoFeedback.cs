using UnityEngine;

/// <summary>
/// Null object: every method of the service, none of them honoured - but this
/// one says so. A game that never crashes is a game that can fail in silence.
/// A plain C# class on purpose - a destroyed MonoBehaviour compares equal to
/// null in Unity, which would bring back the very problem this removes.
/// </summary>
public class NoFeedback : IFeedbackService
{
    public void Hit(Color color)
        => Debug.LogWarning("No FlashManager around: the hit feedback was dropped.");

    public void Clear()
    {
    }
}
