using UnityEngine;
using UnityEngine.SceneManagement;

/// IMGUI on purpose: the demo is about the managers, not about the UI.
public class LevelHud : MonoBehaviour
{
    [SerializeField] private Color hitColor = new Color(1f, 0.82f, 0.1f);
    [SerializeField] private string nextScene = "Level 2";

    private void OnGUI()
    {
        DemoStyle.Apply();

        GUILayout.BeginArea(new Rect(40, 40, 560, 560));
        GUILayout.Label(SceneManager.GetActiveScene().name.ToUpperInvariant());
        GUILayout.Label("Score: " + ScoreKeeper.Instance.Score);
        GUILayout.Space(14);

        if (GUILayout.Button("Hit", GUILayout.Height(48)))
        {
            Feedback.Current.Hit(hitColor);
            ScoreKeeper.Instance.Add(10);
        }

        GUILayout.Space(14);
        if (GUILayout.Button("Next level", GUILayout.Height(48))) SceneManager.LoadScene(nextScene);
        if (GUILayout.Button("Back to the menu", GUILayout.Height(48))) SceneManager.LoadScene("Menu");

        GUILayout.Space(14);
        if (GUILayout.Button("Load the HUD scene on top", GUILayout.Height(44)))
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

        GUILayout.EndArea();
    }
}
