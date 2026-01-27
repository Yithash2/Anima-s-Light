using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "GroundMat", menuName = "Ground Material", order = 0)]
    public class GroundMaterial : ScriptableObject
    {
        public float adhesion = 1; //> 1 => More acceleration Time; < 1 => Way less acceleration Time
        public float friction = 1; //< 1 => Longuer decceleration; > 1 => Shorter decceleration
        public float maxSpeedAddifier = 0;
        
        public bool isWallJumpable = true;
        public bool canHeadBoost = true;
        
    }
}