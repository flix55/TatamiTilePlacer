using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
 
/****************************************
*
*       2020 Alan Mattano
*       SOARING STARS Lab
*
* ***************************************/
 
public class Copy_Path : MonoBehaviour
{
    [MenuItem("Assets/Copy Full Path Location", false, 19)]
    private static void CopyPathToClipboard()
    {
        Object obj = Selection.activeObject;
        if (obj != null)
        {
 
            if (AssetDatabase.Contains(obj))
            {
                string path = AssetDatabase.GetAssetPath(obj);
 
                path = path.TrimStart('A', 's', 's', 'e', 't');
 
                path = Application.dataPath + path;
 
                path = path.Replace('/', '\\');
 
                GUIUtility.systemCopyBuffer = path;
 
                Debug.Log("The full path was copy to the clipboard:\n" + path);
            }
        }
    }
}