using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class SuperSampling : MonoBehaviour
{
    public RenderTexture RenderTexture;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var cam = GetComponent<Camera>();
        cam.targetTexture = null;

        Graphics.Blit(RenderTexture, destination);
    }
}