using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class SuperSampling : MonoBehaviour
{
    public RenderTexture RenderTexture;

    // Start is called before the first frame update

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var cam = GetComponent<Camera>();
        cam.targetTexture = RenderTexture;
        cam.targetTexture = null;

        Graphics.Blit(RenderTexture, destination);
    }
}