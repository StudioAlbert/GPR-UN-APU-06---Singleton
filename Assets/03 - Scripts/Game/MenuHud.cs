using UnityEngine;
using UnityEngine.SceneManagement;

/// IMGUI on purpose: the demo is about the managers, not about the UI.
public class MenuHud : MonoBehaviour
{
    private void OnGUI()
    {
        DemoStyle.Apply();

        GUILayout.BeginArea(new Rect(40, 40, 560, 460));
        GUILayout.Label("MENU");
        GUILayout.Label("Score: " + ScoreKeeper.Instance.Score);
        GUILayout.Space(14);

        if (GUILayout.Button("Play", GUILayout.Height(48))) SceneManager.LoadScene("Level");
        if (GUILayout.Button("Reset the score", GUILayout.Height(48))) ScoreKeeper.Instance.ResetScore();

        GUILayout.EndArea();
    }
}
