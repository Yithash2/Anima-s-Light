using UnityEditor;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;

[AutoCustomTmxImporter()]
public class BetterCollisionLayerImporter : CustomTmxImporter
{
    private static TiledImporterSettings settings;

    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        if (settings == null)
        {
            // Load from Resources or use AssetDatabase if in Editor
            settings = AssetDatabase.LoadAssetAtPath<TiledImporterSettings>("Assets/Editor/TiledImporterSettings.asset");
        }

        if (settings == null)
        {
            Debug.LogWarning("TiledImporterSettings not found. Water material won't be applied.");
            return;
        }

        foreach (var layer in args.ImportedSuperMap.GetComponentsInChildren<SuperLayer>())
        {
            var customProperties = layer.GetComponent<SuperCustomProperties>();
            if (customProperties == null) continue;

            if (customProperties.m_Properties.Any(p =>
                p.m_Name == "unity:collision" && p.m_Value.Equals("Tilemap", System.StringComparison.OrdinalIgnoreCase)))
            {
                if (!layer.TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb = layer.gameObject.AddComponent<Rigidbody2D>();
                }
                rb.bodyType = RigidbodyType2D.Static;

                if (!layer.TryGetComponent<CompositeCollider2D>(out var composite))
                    composite = layer.gameObject.AddComponent<CompositeCollider2D>();
                composite.geometryType = CompositeCollider2D.GeometryType.Polygons;

                if (!layer.TryGetComponent<TilemapCollider2D>(out var tilemapCollider))
                    tilemapCollider = layer.gameObject.AddComponent<TilemapCollider2D>();
                tilemapCollider.usedByComposite = true;

                if (customProperties.m_Properties.Any(p => p.m_Name == "unity:IsTrigger" && p.m_Value.ToLower() == "true"))
                    composite.isTrigger = true;
                else
                    composite.isTrigger = false;

                var cpm = layer.AddComponent<CustomPhysicsMaterials>();
                if (customProperties.m_Properties.Any(p =>
                        p.m_Name == "custom:material" && p.m_Value.ToLower() == "mud"))
                    cpm.material = settings.MudPMat;
                else if (customProperties.m_Properties.Any(p =>
                             p.m_Name == "custom:material" && p.m_Value.ToLower() == "ice"))
                    cpm.material = settings.IcePMat;
                else if (customProperties.m_Properties.Any(p =>
                             p.m_Name == "custom:material" && p.m_Value.ToLower() == "webs"))
                    cpm.material = settings.WebPMat;
                else  cpm.material = settings.DirtPMat;
            }

            if (customProperties.m_Properties.Any(p => p.m_Name == "custom:material" && p.m_Value.ToLower() == "water"))
            {
                var renderer = layer.GetComponent<TilemapRenderer>();
                if (renderer != null && settings.waterMaterial != null)
                {
                    renderer.material = settings.waterMaterial;
                }
            }

            if (customProperties.m_Properties.Any(p => p.m_Name == "unity:tag" && p.m_Value.ToLower() == "deadly"))
            {
                layer.AddComponent<Deadly>();
            }
        }
    }
}
