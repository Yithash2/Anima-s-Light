using Levels;
using UnityEngine;

[CreateAssetMenu(fileName = "TiledImporterSettings", menuName = "Custom/Tiled Importer Settings")]
public class TiledImporterSettings : ScriptableObject
{
    public Material waterMaterial;
    
    public GroundMaterial DirtPMat;
    public GroundMaterial MudPMat;
    public GroundMaterial IcePMat;
    public GroundMaterial WebPMat;
}