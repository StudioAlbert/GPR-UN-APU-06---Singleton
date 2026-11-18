using UnityEngine;

/// <summary>
/// The contract, and nothing more. On its own it guarantees nothing: all it
/// does is make two implementations interchangeable.
/// </summary>
public interface IFeedbackService
{
    void Hit(Color color);

    void Clear();
}
