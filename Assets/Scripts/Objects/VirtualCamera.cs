using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Objects
{
    public class VirtualCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private CinemachineConfiner2D virtualConfiner;
        
        public float Size
        {
            set => virtualCamera.Lens.OrthographicSize = value;
        }

        public GameObject Bounds
        {
            set => virtualConfiner.BoundingShape2D = value.GetComponent<Collider2D>();
        }

        private void OnValidate()
        {
            virtualCamera.Follow = virtualCamera.Follow ? virtualCamera.Follow :  FindFirstObjectByType<PlayerStateMachine>().transform;
        }
    }
} 