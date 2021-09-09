using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelAtributes", menuName = "My ScritableObject/LevelAtributes")]
public class LevelAtribbutesScriptableObject : ScriptableObject
{
    [Header("Camera Spawn Settings")] 
    public float zoomCamera;
    public Vector3 cameraPosition;
    
    //all important variables before you play the game, aka levels
    
}
