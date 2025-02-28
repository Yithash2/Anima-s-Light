using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class cam : MonoBehaviour
{
    #region Input
    GameObject player;
    [field: SerializeField] public Vector3 ofset { get; private set; }
    [SerializeField] float smooth_speed = 0.125f;

    [SerializeField] float moveShiftMultX;
    [SerializeField] float moveShiftMultY;

    [SerializeField] Animator animator;

    public Animator CamAnimator { get{ return animator; } }

    public float verticalShift;

//    [SerializeField] PlayerStateMachine _playerStateMachine;

    float intialOfsetY;
    #endregion

    [field: SerializeField] public GameObject Background { get; private set; }

    void Start(){
        player = PlayerStateMachine.Instance.gameObject;
        intialOfsetY = ofset.y;
    }
    void FixedUpdate()
    {
        #region smooth déplacement
        Vector3 desired_position = player.transform.position + ofset; //position desirer
        Vector3 smooth_position = Vector3.Lerp(transform.position, desired_position, smooth_speed); //On smooth la postion a l'autre
        transform.position = smooth_position;
        #endregion

        #region déplacement avec fleche
        float horizontalShift = Input.GetAxis("Horizontal") * moveShiftMultX; // le changement d'ofset par rapport au déplacement horizontal
        ofset = new Vector3(horizontalShift, verticalShift,ofset.z);
        }
        #endregion
            
    }

