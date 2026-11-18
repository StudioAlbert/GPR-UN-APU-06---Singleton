using UnityEngine;

namespace Singletons
{
    /// <summary>
    /// Every scene carries its manager so that it can be played on its own, and
    /// the newest arrival destroys the older ones. The opposite policy from
    /// chapter 1, where the newcomer was the one that died.
    /// After Adam Myhre, adammyhre/Unity-Utils.
    /// </summary>
    public class RegulatorSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;

        public static T Instance => instance;

        public float InitializationTime { get; private set; }

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
}
