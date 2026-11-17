using UnityEngine;

/// <summary>
/// The singleton plumbing, written once. The accessor finds the instance in the
/// scene, and creates one when nothing is there.
/// After Adam Myhre, adammyhre/Unity-Utils.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public static bool HasInstance => instance != null;

    public static T TryGetInstance() => HasInstance ? instance : null;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindAnyObjectByType<T>();
            if (instance == null)
            {
                var go = new GameObject(typeof(T).Name + " Auto-Generated");
                instance = go.AddComponent<T>();
            }
            return instance;
        }
    }

    // Override it and call base.Awake(), or instance is never assigned.
    protected virtual void Awake()
    {
        if (!Application.isPlaying) return;
        instance = this as T;
    }
}
