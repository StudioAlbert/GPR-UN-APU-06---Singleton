using Demo;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chapter3NullObject
{
    /// IMGUI on purpose: the demo is about the managers, not about the UI.
    public class LevelHud : MonoBehaviour
    {
        [SerializeField] private AudioClip shot;
        [SerializeField] private string nextScene;
        [SerializeField] private string hudScene;

        private void OnGUI()
        {
            DemoStyle.Apply();

            GUILayout.BeginArea(new Rect(40, 40, 580, 620));
            GUILayout.Label(SceneManager.GetActiveScene().name.ToUpperInvariant());
            GUILayout.Label("Score: " + ScoreKeeper.Instance.Score);
            GUILayout.Space(14);

            if (GUILayout.Button("Shoot", GUILayout.Height(48)))
            {
                Sound.Current.Play(shot);
                ScoreKeeper.Instance.Add(10);
            }

            GUILayout.Space(14);
            if (GUILayout.Button("Next level", GUILayout.Height(48))) SceneManager.LoadScene(nextScene);
            if (GUILayout.Button("Load the HUD scene on top", GUILayout.Height(44)))
            {
                SceneManager.LoadScene(hudScene, LoadSceneMode.Additive);
            }

            GUILayout.Space(14);
            if (GUILayout.Button("Reset the score", GUILayout.Height(40))) ScoreKeeper.Instance.ResetScore();

            GUILayout.EndArea();
        }
    }
}
