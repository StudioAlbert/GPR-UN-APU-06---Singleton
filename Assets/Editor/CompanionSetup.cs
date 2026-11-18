using System;
using Demo;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Builds the demo scenes from scratch, so the companion can be rebuilt from
/// source at any time. Four scenes per chapter, all in one branch: the scripts
/// of a chapter live in their own folder and their own namespace, and the
/// scenes of a chapter only ever reference that chapter's components.
/// Run it from the menu, or in batch mode with
/// -executeMethod CompanionSetup.BuildAll.
/// </summary>
public static class CompanionSetup
{
    private const string ScenesFolder = "Assets/01 - Scenes";

    private const string MusicPath = "Assets/05 - Audio/music.wav";
    private const string ShotPath = "Assets/05 - Audio/shot.wav";

    private static readonly (string Prefix, string Folder, string Namespace)[] Chapters =
    {
        ("01", "01 - Basic", "Chapter1Basic"),
        ("02", "02 - Generic", "Chapter2Generic"),
        ("03", "03 - Null object", "Chapter3NullObject"),
        ("04", "04 - Persistent", "Chapter4Persistent")
    };

    [MenuItem("Companion/Rebuild the demo scenes")]
    public static void BuildAll()
    {
        AudioClip music = AssetDatabase.LoadAssetAtPath<AudioClip>(MusicPath);
        AudioClip shot = AssetDatabase.LoadAssetAtPath<AudioClip>(ShotPath);

        var built = new System.Collections.Generic.List<EditorBuildSettingsScene>();

        foreach ((string prefix, string folder, string ns) in Chapters)
        {
            EnsureFolder(folder);
            foreach (string path in BuildChapter(prefix, folder, ns, music, shot))
            {
                built.Add(new EditorBuildSettingsScene(path, true));
            }
        }

        EditorBuildSettings.scenes = built.ToArray();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("CompanionSetup: " + built.Count + " scenes written in " + ScenesFolder + ".");
    }

    private static string[] BuildChapter(string prefix, string folder, string ns, AudioClip music, AudioClip shot)
    {
        Type manager = Find(ns + ".AudioManager");
        Type score = Find(ns + ".ScoreKeeper");
        Type hud = Find(ns + ".LevelHud");

        string root = ScenesFolder + "/" + folder + "/" + prefix + " - ";
        string bootstrap = root + "Bootstrap.unity";
        string level = root + "Level.unity";
        string level2 = root + "Level 2.unity";
        string additive = root + "HUD.unity";

        Scene scene = FreshScene(true);
        AddManagers("Managers", manager, score, music, shot);
        BootstrapLoader loader = new GameObject("Bootstrap").AddComponent<BootstrapLoader>();
        SetString(loader, "firstScene", prefix + " - Level");
        EditorSceneManager.SaveScene(scene, bootstrap);

        // The managers sit in the level, the way everyone writes them first. Level 2 has
        // none: it is the next level nobody equipped, and the whole session starts there.
        BuildLevel(level, hud, shot, prefix + " - Level 2", prefix + " - HUD", manager, score, music, true);
        BuildLevel(level2, hud, shot, prefix + " - Level", prefix + " - HUD", manager, score, music, false);

        scene = FreshScene(false);
        AddManagers("Managers (from the HUD scene)", manager, score, music, shot);
        EditorSceneManager.SaveScene(scene, additive);

        return new[] { bootstrap, level, level2, additive };
    }

    private static void BuildLevel(string path, Type hud, AudioClip shot, string nextScene,
        string hudScene, Type manager, Type score, AudioClip music, bool withManagers)
    {
        Scene scene = FreshScene(true);
        if (withManagers) AddManagers("Managers", manager, score, music, shot);

        Component component = new GameObject("LevelHud").AddComponent(hud);
        SetObject(component, "shot", shot);
        SetString(component, "nextScene", nextScene);
        SetString(component, "hudScene", hudScene);
        EditorSceneManager.SaveScene(scene, path);
    }

    private static Type Find(string name)
    {
        Type type = Type.GetType(name + ", Assembly-CSharp");
        if (type == null) throw new InvalidOperationException("CompanionSetup: no type named " + name + ".");
        return type;
    }

    private static void EnsureFolder(string folder)
    {
        if (!AssetDatabase.IsValidFolder(ScenesFolder)) AssetDatabase.CreateFolder("Assets", "01 - Scenes");
        if (!AssetDatabase.IsValidFolder(ScenesFolder + "/" + folder)) AssetDatabase.CreateFolder(ScenesFolder, folder);
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

        go.AddComponent<AudioListener>();
    }

    private static void AddManagers(string name, Type manager, Type score, AudioClip music, AudioClip shot)
    {
        var go = new GameObject(name);

        Component audio = go.AddComponent(manager);
        AudioSource source = go.GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = 0.5f;

        SetObject(audio, "music", music);
        SetObject(audio, "shot", shot);

        go.AddComponent(score);
    }

    private static void SetObject(UnityEngine.Object target, string field, UnityEngine.Object value)
    {
        var serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) return;

        property.objectReferenceValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetString(UnityEngine.Object target, string field, string value)
    {
        var serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(field);
        if (property == null) return;

        property.stringValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }
}
