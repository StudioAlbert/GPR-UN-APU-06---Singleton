using UnityEngine;

namespace Singletons
{
    /// <summary>
    /// Survives a scene change. DontDestroyOnLoad only moves a root object,
    /// hence the unparenting. Here the first instance wins and later ones are
    /// destroyed. After Adam Myhre, adammyhre/Unity-Utils.
    /// </summary>
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private bool autoUnparentOnAwake = true;

        protected static T instance;

        public static bool HasInstance => instance != null;

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

            if (autoUnparentOnAwake) transform.SetParent(null);

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
