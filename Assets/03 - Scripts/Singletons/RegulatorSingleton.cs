using UnityEngine;

/// <summary>
/// The newest instance wins: on awake it destroys every older one it finds.
/// Lets every scene carry its own manager and still be played on its own.
/// After Adam Myhre, adammyhre/Unity-Utils.
/// </summary>
public class RegulatorSingleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public static bool HasInstance => instance != null;

    public float InitializationTime { get; private set; }

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

    protected virtual void Awake()
    {
        if (!Application.isPlaying) return;

        InitializationTime = Time.time;
        DontDestroyOnLoad(gameObject);

        foreach (T old in FindObjectsByType<T>())
        {
            var regulator = old.GetComponent<RegulatorSingleton<T>>();
            if (regulator != null && regulator.InitializationTime < InitializationTime)
            {
                Debug.Log("RegulatorSingleton: destroying the older " + typeof(T).Name + ".", old);
                Destroy(old.gameObject);
            }
        }

        if (instance == null) instance = this as T;
    }
}
