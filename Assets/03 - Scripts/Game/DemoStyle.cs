using UnityEngine;

/// Readable IMGUI on a projector.
public static class DemoStyle
{
    public static void Apply()
    {
        GUI.skin.label.fontSize = 24;
        GUI.skin.button.fontSize = 24;
        GUI.skin.label.fontStyle = FontStyle.Bold;
    }
}
