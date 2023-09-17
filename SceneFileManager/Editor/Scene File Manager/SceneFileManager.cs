using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.Collections.Generic;
using System.IO;
using Random = UnityEngine.Random;

public class SceneFileManager : EditorWindow
{
    private Dictionary<string, Color> sceneFiles = new Dictionary<string, Color>();
    private SceneFileManagerData m_sceneData;
    private SceneFileManagerColors colorsData;
    
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        ShowWindow();
    }

    [MenuItem("Window/Scene File Manager")]
    public static void ShowWindow()
    {
        SceneFileManager window = GetWindow<SceneFileManager>("Scene File Manager");
        window.LoadSceneFiles();
    }

    private void LoadSceneFiles()
    {
        sceneFiles.Clear();
        m_sceneData = Resources.Load<SceneFileManagerData>("SceneFileManagerNames");
        colorsData = Resources.Load<SceneFileManagerColors>("SceneFileManagerColors");
        foreach (var sceneName in m_sceneData.sceneFilesNames)
        {
            Color color = m_sceneData.GetColorBySceneName(sceneName);
            Color.RGBToHSV(color, out float H, out float S, out float v);
            sceneFiles.Add(sceneName, Color.HSVToRGB(H, S, v));
        }
    }

    private void OnGUI()
    {   
        GUILayout.Label("Scene Files", EditorStyles.boldLabel);

        Event evt = Event.current;

        if (evt.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            Event.current.Use();
        }
        else if (evt.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();
            foreach (string draggedObject in DragAndDrop.paths)
            {
                if (draggedObject.EndsWith(".unity"))
                {
                    string sceneName = Path.GetFileNameWithoutExtension(draggedObject);
                    Color randomColor = PickRandomColorFromData();
                    Color.RGBToHSV(randomColor, out float H, out float S, out float v);
                    m_sceneData.AddScene(sceneName, randomColor);
                    sceneFiles.Add(sceneName,  Color.HSVToRGB(H, S, v));
                }
            }
            Event.current.Use();
        }

        foreach (var scene in sceneFiles)
        {
            GUILayout.BeginHorizontal();

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.normal.textColor = scene.Value;
            GUILayout.Label(scene.Key, labelStyle);

            if (GUILayout.Button("Load", GUILayout.Width(60)))
            {
                LoadScene(scene.Key);
            }

            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                sceneFiles.Remove(scene.Key);
                m_sceneData.RemoveScene(scene.Key); //TODO: change this so it won't use scene name as an identifier to delete a scene
                GUILayout.EndHorizontal();
                break;
            }

            GUILayout.EndHorizontal();
        }
    }

    private Color PickRandomColorFromData()
    {
        var colors = colorsData.sceneColors;
        var randomInt = Random.Range(0, colors.Count);
        return colors[randomInt];
    }

    private void LoadScene(string sceneName)
    {
        string scenePath = FindScenePathByName(sceneName);
        if (!string.IsNullOrEmpty(scenePath))
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
    
    private string FindScenePathByName(string sceneName)
    {
        string[] guids = AssetDatabase.FindAssets("t:Scene");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (Path.GetFileNameWithoutExtension(path) == sceneName)
            {
                return path;
            }
        }
        return null;
    }
}