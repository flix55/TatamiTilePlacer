using UnityEngine;
 
[ExecuteAlways]
public class CustomImageEffect : MonoBehaviour {
 
    public Material material;
 
    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, material);
    }
}