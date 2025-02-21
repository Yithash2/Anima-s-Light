using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/PlayerData")]
public class PlayerDateScriptableObject : ScriptableObject
{
    [SerializeField] float initalJumpHeight;
    [SerializeField] float wallJumpHeight;
    [SerializeField] float wallJumpLength;

    #region Jump
    [SerializeField] float gravityMult;
    [SerializeField] float groundSlamMult;
    [SerializeField] float clampedVelocity;
    [SerializeField] float slideSpeed;

    [Header ("Timers")]
    [SerializeField] float _wallJumpControlTime;
    float _wallJumpCounter;
    #endregion
    
    #region Run
    [SerializeField] private float _runAcceleration;
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _linearDrag;


    #endregion

    #region Get Sets
    public float WallJumpControlTime {get {return _wallJumpControlTime;} }
    public float GroundSlamMult {get {return groundSlamMult;}}
    public float SlideSpeed {get {return slideSpeed;}}
    public float ClampedVelocity {get {return clampedVelocity;}}
    public float GravityMult {get {return gravityMult;}}
    public float InitalJumpHeight {get {return initalJumpHeight;}}
    public float WallJumpHeight {get {return wallJumpHeight;}}
    public float WallJumpLength {get {return wallJumpLength;}}
    public float GroundAccelTime {get {return _runAcceleration;}}
    public float AirAccelTime {get {return _airAcceleration;}}
    public float MaxSpeed {get {return _maxSpeed;}}
    public float LinearDrag {get {return _linearDrag;}}

    [field:SerializeField]public float ReleaseCoyoteTimer { get; private set; }
    #endregion

}
