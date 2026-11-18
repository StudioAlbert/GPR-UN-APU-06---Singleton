using Demo;
using UnityEngine;

namespace Chapter2Generic
{
    /// The HUD that the additive scene brings along. It reads the score through the
    /// same singleton the level uses, so what it shows tells you which manager
    /// answered -- the level's one, or the one this scene carried in.
    public class ScoreHud : MonoBehaviour
    {
        private void OnGUI()
        {
            DemoStyle.Apply();

            GUILayout.BeginArea(new Rect(Screen.width - 340f, 40f, 300f, 140f));
            GUILayout.Label("HUD SCENE");
            GUILayout.Label("Score: " + ScoreKeeper.Instance.Score);
            GUILayout.EndArea();
        }
    }
}
