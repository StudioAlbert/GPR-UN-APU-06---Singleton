using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Builds the four demo scenes from scratch, so the companion can be rebuilt
/// from source at any time. Run it from the menu, or in batch mode with
/// -executeMethod CompanionSetup.BuildAll.
/// </summary>
public static class CompanionSetup
{
    private const string ScenesFolder = "Assets/01 - Scenes";

    private const string MusicPath = "Assets/05 - Audio/music.wav";
    private const string ShotPath = "Assets/05 - Audio/shot.wav";

    [MenuItem("Companion/Rebuild the demo scenes")]
    public static void BuildAll()
    {
        if (!AssetDatabase.IsValidFolder(ScenesFolder))
        {
            AssetDatabase.CreateFolder("Assets", "01 - Scenes");
        }

        AudioClip music = AssetDatabase.LoadAssetAtPath<AudioClip>(MusicPath);
        AudioClip shot = AssetDatabase.LoadAssetAtPath<AudioClip>(ShotPath);

        BuildBootstrap(music, shot);
        BuildMenu();
        BuildLevel(shot);
        BuildHud(music, shot);

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(ScenesFolder + "/Bootstrap.unity", true),
            new EditorBuildSettingsScene(ScenesFolder + "/Menu.unity", true),
            new EditorBuildSettingsScene(ScenesFolder + "/Level.unity", true),
            new EditorBuildSettingsScene(ScenesFolder + "/HUD.unity", true)
        };

        string sample = "Assets/Scenes/SampleScene.unity";
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(sample) != null)
        {
            AssetDatabase.DeleteAsset(sample);
            AssetDatabase.DeleteAsset("Assets/Scenes");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("CompanionSetup: four scenes written in " + ScenesFolder + ".");
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

    private static GameObject AddManagers(string name, AudioClip music, AudioClip shot)
    {
        var go = new GameObject(name);

        AudioManager audio = go.AddComponent<AudioManager>();
        AudioSource source = go.GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = 0.5f;

        SetClip(audio, "music", music);
        SetClip(audio, "shot", shot);

        go.AddComponent<ScoreKeeper>();
        return go;
    }

    private static void SetClip(Object target, string field, AudioClip clip)
    {
        var serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) return;          // the branch may not carry that field

        property.objectReferenceValue = clip;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void BuildBootstrap(AudioClip music, AudioClip shot)
    {
        Scene scene = FreshScene(true);
        AddManagers("Managers", music, shot);
        new GameObject("Bootstrap").AddComponent<BootstrapLoader>();
        EditorSceneManager.SaveScene(scene, ScenesFolder + "/Bootstrap.unity");
    }

    private static void BuildMenu()
    {
        Scene scene = FreshScene(true);
        new GameObject("MenuHud").AddComponent<MenuHud>();
        EditorSceneManager.SaveScene(scene, ScenesFolder + "/Menu.unity");
    }

    private static void BuildLevel(AudioClip shot)
    {
        Scene scene = FreshScene(true);
        LevelHud hud = new GameObject("LevelHud").AddComponent<LevelHud>();
        SetClip(hud, "shot", shot);
        EditorSceneManager.SaveScene(scene, ScenesFolder + "/Level.unity");
    }

    private static void BuildHud(AudioClip music, AudioClip shot)
    {
        Scene scene = FreshScene(false);
        AddManagers("Managers (from the HUD scene)", music, shot);
        EditorSceneManager.SaveScene(scene, ScenesFolder + "/HUD.unity");
    }
}
