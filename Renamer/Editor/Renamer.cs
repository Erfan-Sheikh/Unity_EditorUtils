using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

public class Renamer : EditorWindow
{
    private List<Object> objects;
    private string prefix = "";
    private string suffix = "";
    private string newName = "";
    private string replacei = "";
    private string replaceWith = "";

    [MenuItem("Art Tools/Renamer")]
    static void CreateRenamer()
    {
        GetWindow<Renamer>();
    }

    public void ChangeName()
    {
        var activeObjects = Selection.objects;
        var ID = 0;
        foreach (var activeObject in activeObjects)
        {
            var path = AssetDatabase.GetAssetPath(activeObject);

            AssetDatabase.RenameAsset(path, newName + ID);

            ID++;
        }
    }

    public void PrefixAndSuffix()
    {
        var activeObjects = Selection.objects;
        foreach (var activeObject in activeObjects)
        {
            var path = AssetDatabase.GetAssetPath(activeObject);
            var name = activeObject.name;
            AssetDatabase.RenameAsset(path, prefix + name + suffix);
        }
    }

    public void ReplaceWith()
    {
        if (replaceWith.Length <= 0 && replacei.Length <= 0)
            return;

        var activeObjects = Selection.objects;
        foreach (var activeObject in activeObjects)
        {
            var path = AssetDatabase.GetAssetPath(activeObject);
            var name = activeObject.name;
            name = name.Replace(replacei, replaceWith);

            AssetDatabase.RenameAsset(path, name);
        }
    }

    private void OnGUI()
    {
        newName = EditorGUILayout.TextField("New Name", newName);
        if (GUILayout.Button("Change Name"))
        {
            ChangeName();
            newName = "";
            Repaint();
        }

        GUILayout.BeginHorizontal(); // Start a horizontal layout group
        prefix = EditorGUILayout.TextField("Prefix", prefix);
        suffix = EditorGUILayout.TextField("Suffix", suffix);
        GUILayout.EndHorizontal(); // End the horizontal layout group

        GUILayout.BeginHorizontal(); // Start a horizontal layout group
        replacei = EditorGUILayout.TextField("Replace", replacei);
        replaceWith = EditorGUILayout.TextField("With", replaceWith);
        GUILayout.EndHorizontal(); // End the horizontal layout group

        if (GUILayout.Button("Replace Names"))
        {
            PrefixAndSuffix();
            ReplaceWith();
            ClearNames();
            Repaint();
        }
    }

    private void ClearNames()
    {
        prefix = "";
        suffix = "";
        replacei = "";
        replaceWith = "";
    }
}