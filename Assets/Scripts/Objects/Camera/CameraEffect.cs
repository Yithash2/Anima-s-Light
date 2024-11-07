using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class HueShiftEffect : MonoBehaviour
{
    public Shader hueShiftShader; // Reference to the hue shift shader
    private Material hueShiftMaterial; // Material to hold the shader

    [Range(0, 1)]
    public float hueShift = 0.0f; // Property to control the hue shift

    void OnEnable()
    {
        // Create material if shader is assigned
        if (hueShiftShader == null)
        {
            Debug.LogError("Hue Shift Shader not assigned!");
            return;
        }

        hueShiftMaterial = new Material(hueShiftShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (hueShiftMaterial != null)
        {
            // Set the hue shift value
            hueShiftMaterial.SetFloat("_HueShift", hueShift);
            // Apply the shader effect to the camera's output
            Graphics.Blit(source, destination, hueShiftMaterial);
        }
        else
        {
            Graphics.Blit(source, destination); // Just copy if no material
        }
    }

    void OnDisable()
    {
        // Clean up the material on disable
        if (hueShiftMaterial != null)
        {
            DestroyImmediate(hueShiftMaterial);
        }
    }
}

