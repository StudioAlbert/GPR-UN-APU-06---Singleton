using UnityEngine;

namespace Demo
{
    /// <summary>
    /// A full screen flash. It does not replace the sound, it confirms it: a
    /// classroom projector has no speakers, and the back of the room has to
    /// see that the call reached someone. Owned by whoever plays the clip.
    /// </summary>
    public class Flash
    {
        private const float FadeSpeed = 2.6f;

        private readonly Color tint = new Color(1f, 0.82f, 0.1f);

        private Texture2D pixel;
        private float alpha;

        public void Fire() => alpha = 0.55f;

        public void Fade(float deltaTime) => alpha = Mathf.Max(0f, alpha - FadeSpeed * deltaTime);

        public void Draw()
        {
            if (alpha <= 0f) return;

            if (pixel == null)
            {
                pixel = new Texture2D(1, 1);
                pixel.SetPixel(0, 0, Color.white);
                pixel.Apply();
            }

            GUI.depth = 10;         // behind the HUD, over the game
            GUI.color = new Color(tint.r, tint.g, tint.b, alpha);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), pixel);
            GUI.color = Color.white;
        }
    }
}
