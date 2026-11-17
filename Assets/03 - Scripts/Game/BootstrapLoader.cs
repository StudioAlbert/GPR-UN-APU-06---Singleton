using UnityEngine;
using UnityEngine.SceneManagement;

/// The only place that creates the services, once, before the game exists.
public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string firstScene = "Menu";

    private void Start() => SceneManager.LoadScene(firstScene);
}
