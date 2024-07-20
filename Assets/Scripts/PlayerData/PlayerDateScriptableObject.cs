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
    [SerializeField] float acc;
    [SerializeField] float _airVelocity;
    [SerializeField] float _groundVelocity;
    [SerializeField] float _runAcceleration;
    [SerializeField] float _runDecceleration;
    [SerializeField] float _airMult;

    #endregion

    #region Get Sets
    public float WallJumpControlTime {get {return _wallJumpControlTime;} }
    public float GroundSlamMult {get {return groundSlamMult;}}
    public float GroundVelocity {get {return _groundVelocity;}}
    public float AirVelocity {get {return _airVelocity;}}
    public float AirMult {get {return _airMult;}}
    public float RunAcceleration {get {return _runAcceleration;}}
    public float RunDecceleration {get {return _runDecceleration;}}
    public float SlideSpeed {get {return slideSpeed;}}
    public float Accel {get {return acc;}}
    public float ClampedVelocity {get {return clampedVelocity;}}
    public float GravityMult {get {return gravityMult;}}
    public float InitalJumpHeight {get {return initalJumpHeight;}}
    public float WallJumpHeight {get {return wallJumpHeight;}}
    public float WallJumpLength {get {return wallJumpLength;}}
    #endregion

}
