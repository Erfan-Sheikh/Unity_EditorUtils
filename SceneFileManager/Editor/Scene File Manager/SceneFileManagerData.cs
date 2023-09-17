using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SceneFileManagerData", menuName = "ErfanSh/SceneFileManager/Scene File Manager Data")]
public class SceneFileManagerData : ScriptableObject
{
    public List<String> sceneFilesNames;
    public List<Color> sceneFilesColors;

    public void AddScene(string mSceneName, Color mSceneColor)
    {
        sceneFilesNames.Add(mSceneName);
        sceneFilesColors.Add(mSceneColor);
    }
    
    public void RemoveScene(string mSceneName)
    {
        for (var i = 0; i < sceneFilesNames.Count; i++)
        {
            var sceneName = sceneFilesNames[i];
            if (sceneName == mSceneName)
            {
                sceneFilesNames.Remove(sceneName);
                sceneFilesColors.Remove(sceneFilesColors[i]);
                return;
            }
        }
    }

    public Color GetColorBySceneName(string sceneName)
    {
        for (var i = 0; i < sceneFilesNames.Count; i++)
        {
            if (sceneFilesNames[i] == sceneName)
            {
                return sceneFilesColors[i];
            }
        }

        return sceneFilesColors[0];
    }
}