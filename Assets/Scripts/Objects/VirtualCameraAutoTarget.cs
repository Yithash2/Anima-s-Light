using Unity.Cinemachine;
using UnityEngine;

public class VirtualCameraAutoTarget : MonoBehaviour
{
    void OnValidate()
    {
        var cC = GetComponent<CinemachineCamera>();
        cC.Follow = FindFirstObjectByType<PlayerStateMachine>().transform;
    }
}
