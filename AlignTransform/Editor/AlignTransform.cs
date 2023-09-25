using System.Linq;
using UnityEditor;
using UnityEngine;

public class AlignTransform : MonoBehaviour
{
    [MenuItem("Art Tools/Align Transform", false, 0)]
    static void PerformTransformAlign()
    {
        var selectedObjects = Selection.objects.OfType<GameObject>().ToArray();
        var lastTf = selectedObjects.Last().transform;


        for (var i = 0; i < selectedObjects.Length - 1; i++)
        {
            var selectedObject = selectedObjects[i]; 
            var currentTf = selectedObject.transform;
            Undo.RecordObject(currentTf, "Align transform " + currentTf.name + " with " + lastTf.name);
            currentTf.SetPositionAndRotation(lastTf.position, lastTf.rotation);
        }
    }
}