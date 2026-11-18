using UnityEngine;

/// <summary>
/// The service the whole session is about: a full screen flash, so that the
/// back of the room can tell whether the call reached anyone.
/// Persistent, and it registers itself, so no caller ever names this class.
/// </summary>
public class FlashManager : PersistentSingleton<FlashManager>, IFeedbackService
{
    [SerializeField] private float fadeSpeed = 2.6f;

    private Texture2D pixel;
    private Color tint;
    private float alpha;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;   // a duplicate: base.Awake already condemned it

        MakePixel();
        Feedback.Provide(this);
    }

    private void OnDestroy()
    {
        if (ReferenceEquals(Feedback.Current, this)) Feedback.Provide(null);
    }

    public void Hit(Color color)
    {
        tint = color;
        alpha = 0.6f;
    }

    public void Clear() => alpha = 0f;

    private void Update() => alpha = Mathf.Max(0f, alpha - fadeSpeed * Time.deltaTime);

    private void OnGUI()
    {
        if (alpha <= 0f) return;

        GUI.depth = 10;         // behind the HUD, over the game
        GUI.color = new Color(tint.r, tint.g, tint.b, alpha);
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), pixel);
        GUI.color = Color.white;
    }

    private void MakePixel()
    {
        pixel = new Texture2D(1, 1);
        pixel.SetPixel(0, 0, Color.white);
        pixel.Apply();
    }
}
