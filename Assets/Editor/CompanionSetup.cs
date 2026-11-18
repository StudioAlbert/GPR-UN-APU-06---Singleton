using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Builds the demo scenes from scratch, so the companion can be rebuilt from
/// source at any time. Run it from the menu, or in batch mode with
/// -executeMethod CompanionSetup.BuildAll.
/// </summary>
public static class CompanionSetup
{
    private const string ScenesFolder = "Assets/01 - Scenes";

    [MenuItem("Companion/Rebuild the demo scenes")]
    public static void BuildAll()
    {
        if (!AssetDatabase.IsValidFolder(ScenesFolder))
        {
            AssetDatabase.CreateFolder("Assets", "01 - Scenes");
        }

        BuildBootstrap();
        BuildMenu();
        BuildLevel("Level", "Level 2");
        BuildLevel("Level 2", "Level");
        BuildHud();

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(ScenesFolder + "/Bootstrap.unity", true),
            new EditorBuildSettingsScene(ScenesFolder + "/Menu.unity", true),
            new EditorBuildSettingsScene(ScenesFolder + "/Level.unity", true),
            new EditorBuildSettingsScene(ScenesFolder + "/Level 2.unity", true),
            new EditorBuildSettingsScene(ScenesFolder + "/HUD.unity", true)
        };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("CompanionSetup: five scenes written in " + ScenesFolder + ".");
    }

    private static Scene FreshScene(bool withCamera)
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        if (withCamera) AddCamera();
        return scene;
    }

    private static void AddCamera()
    {
        var go = new GameObject("Main Camera");
        go.tag = "MainCamera";
        go.transform.position = new Vector3(0f, 0f, -10f);

        Camera camera = go.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.09f, 0.09f, 0.11f, 1f);
    }

    private static GameObject AddManagers(string name)
    {
        var go = new GameObject(name);
        go.AddComponent<FlashManager>();
        go.AddComponent<ScoreKeeper>();
        return go;
    }

    private static void SetString(Object target, string field, string value)
    {
        var serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) return;          // the branch may not carry that field

        property.stringValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void BuildBootstrap()
    {
        Scene scene = FreshScene(true);
        AddManagers("Managers");
        new GameObject("Bootstrap").AddComponent<BootstrapLoader>();
        EditorSceneManager.SaveScene(scene, ScenesFolder + "/Bootstrap.unity");
    }

    private static void BuildMenu()
    {
        Scene scene = FreshScene(true);
        new GameObject("MenuHud").AddComponent<MenuHud>();
        EditorSceneManager.SaveScene(scene, ScenesFolder + "/Menu.unity");
    }

    private static void BuildLevel(string sceneName, string nextScene)
    {
        Scene scene = FreshScene(true);
        LevelHud hud = new GameObject("LevelHud").AddComponent<LevelHud>();
        SetString(hud, "nextScene", nextScene);
        EditorSceneManager.SaveScene(scene, ScenesFolder + "/" + sceneName + ".unity");
    }

    private static void BuildHud()
    {
        Scene scene = FreshScene(false);
        AddManagers("Managers (from the HUD scene)");
        EditorSceneManager.SaveScene(scene, ScenesFolder + "/HUD.unity");
    }
}
