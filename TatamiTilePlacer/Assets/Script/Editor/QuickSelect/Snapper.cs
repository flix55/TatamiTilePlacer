using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Snapper
{
    private const string UNDO_STR_SNAP = "snap objects";
    
    [MenuItem("Edit/snap selected object %&S")]
    public static bool SnapTheThingsValidate()
    {
        SnapTheThings();
        return Selection.gameObjects.Length > 0;
    }
    public static void SnapTheThings()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Undo.RecordObject(go.transform, UNDO_STR_SNAP);
            go.transform.position = go.transform.position.Round();
        }
    }
    
    [MenuItem("Tools/Quick select/Select Camera %&w")]
    public static bool ValidateSelection()
    {
        Camera cam = Camera.main;
        return Selection.activeObject = cam;
    }
    
}
