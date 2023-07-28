using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CameraSetup : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        // Set camera up to look at the target
        transform.position = target.position + new Vector3(0f, 5f, -10f);
        transform.LookAt(target.position);
        Camera.main.fieldOfView = 60f;
        Camera.main.nearClipPlane = 0.1f;
        Camera.main.farClipPlane = 100f;
    }
}

public class WebGLBuilder
{
    public static void EmptyBuildWebGL()
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
    }

    [MenuItem("Build/Build WebGL")]
    public static void BuildWebGL()
    {
        // Creating scene dynamically so create the parent directory if it does
        // not exist
        string scenesDirectory = "Assets/Scenes";
        if (!AssetDatabase.IsValidFolder(scenesDirectory))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }

        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject rootObject = new GameObject("RootObject");
        EditorSceneManager.SetActiveScene(newScene);

        GameEntry gameEntry = rootObject.AddComponent<GameEntry>();

        GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeObject.name = "Cube";

        cubeObject.transform.SetParent(rootObject.transform);

        GameObject cameraObject = new GameObject("Camera");
        Camera camera = cameraObject.AddComponent<Camera>();

        cameraObject.transform.position = new Vector3(0f, 0f, -10f);
        cameraObject.transform.rotation = Quaternion.identity;

        camera.clearFlags = CameraClearFlags.Color;
        camera.backgroundColor = Color.black;

        CameraSetup cameraSetup = cameraObject.AddComponent<CameraSetup>();
        cameraSetup.target = cubeObject.transform;

        //EditorSceneManager.MoveGameObjectToScene(cameraObject, newScene);

        string scenePath = Path.Combine(scenesDirectory, "NewScene.unity");
        EditorSceneManager.SaveScene(newScene, scenePath);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new [] { newScene.path };
        buildPlayerOptions.locationPathName = "Builds/WebGL";
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary buildSummary = buildReport.summary;

        if (buildSummary.result == BuildResult.Succeeded)
        {
            Debug.Log("WebGL build succeeded: " + buildSummary.totalSize + " bytes");
        }
        else
        {
            Debug.LogError("WebGL build result: " + buildSummary.result.ToString());
        }
    }

}
