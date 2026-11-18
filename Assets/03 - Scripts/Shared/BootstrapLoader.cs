using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    /// <summary>
    /// The only place that creates the services, once, before the game exists.
    /// Chapter agnostic: the scene it loads is set per chapter in the Inspector.
    /// </summary>
    public class BootstrapLoader : MonoBehaviour
    {
        [SerializeField] private string firstScene;

        private void Start() => SceneManager.LoadScene(firstScene);
    }
}
