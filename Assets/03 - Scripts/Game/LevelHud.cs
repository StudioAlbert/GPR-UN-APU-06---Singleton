using UnityEngine;
using UnityEngine.SceneManagement;

/// IMGUI on purpose: the demo is about the managers, not about the UI.
public class LevelHud : MonoBehaviour
{
    [SerializeField] private AudioClip shot;

    private void OnGUI()
    {
        DemoStyle.Apply();

        GUILayout.BeginArea(new Rect(40, 40, 560, 520));
        GUILayout.Label("LEVEL");
        GUILayout.Label("Score: " + ScoreKeeper.Instance.Score);
        GUILayout.Space(14);

        if (GUILayout.Button("Shoot", GUILayout.Height(48)))
        {
            Sound.Current.Play(shot);
            ScoreKeeper.Instance.Add(10);
        }

        if (GUILayout.Button("Back to the menu", GUILayout.Height(48))) SceneManager.LoadScene("Menu");

        GUILayout.Space(14);
        if (GUILayout.Button("Load the HUD scene on top", GUILayout.Height(44)))
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }

        GUILayout.EndArea();
    }
}
