using System.Linq;
using UnityEditor;
using UnityEngine;

public class ArtUtilsMenu : MonoBehaviour
{
    [MenuItem("Art Tools/Align Transform", false, 0)]
    static void PerformTransformAlign()
    {
        var selectedObjects = Selection.objects.OfType<GameObject>().ToArray();
        var src = selectedObjects.Last().transform;
        var dst = selectedObjects[selectedObjects.Length - 2].transform;
        Undo.RecordObject(dst, "Align transform " + dst.name + " with " + src.name);
            dst.SetPositionAndRotation(src.position, src.rotation);
    }
}
