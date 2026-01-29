using System;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using Objects;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


// The AutoCustomTmxImporterAttribute will force this importer to always be applied.
// Leave this attribute off if you want to choose a custom importer from a drop-down list instead.
[AutoCustomTmxImporter()]
public class CinemachineCamerasImporter : CustomTmxImporter
{
    
    private GameObject virtualCameraPrefab ;
    
    public override void TmxAssetImported(TmxAssetImportedArgs args)
    {
        var map = args.ImportedSuperMap;
        
        virtualCameraPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/VirtualCamera.prefab");
        Debug.Log(virtualCameraPrefab);
        
        SuperLayer cameraBounds = null;
        foreach (var layer in map.GetComponentsInChildren<SuperLayer>())
        {
            if (layer.name == "Camera")
            {
                cameraBounds = layer.GetComponentInChildren<SuperLayer>();
            }
        }

        if (!cameraBounds)
        {
            Debug.LogWarning("No camera bounds found");
            return;
        }

        var cameraRoot = Object.Instantiate(new GameObject("CameraRoots"), map.transform);

        int i = 0;
        foreach (var obj in cameraBounds.GetComponentsInChildren<SuperObject>())
        {
            var room = obj.gameObject.AddComponent<CamRoom>();
            var virtualCamera = Object.Instantiate(virtualCameraPrefab, obj.transform.position, obj.transform.rotation,
                cameraRoot.transform);
            
            virtualCamera.name = $"VirtualCamera {i}";
            virtualCamera.AddComponent<VirtualCameraAutoTarget>();
            var virtualCameraConfiner = virtualCamera.GetComponent<CinemachineConfiner2D>();

            virtualCameraConfiner.BoundingShape2D = obj.GetComponent<Collider2D>();

            room.virtualCamera = virtualCamera;
            
            virtualCamera.SetActive(false);
            i++;
        }
        
        Debug.LogFormat("Map '{0}' has been imported.", map.name);
    }
}
