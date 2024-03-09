using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class cam : MonoBehaviour
{
    #region Input
    [SerializeField] GameObject player;
    [SerializeField] Vector3 ofset;
    [SerializeField] float smooth_speed= 0.125f;

    Rigidbody2D rb;
    [SerializeField] float Timer = 2f;

    float elapsed = 0f;

    [SerializeField] float moveShiftMultX;
    [SerializeField] float moveShiftMultY;

    float intialOfsetY;
    #endregion

    void Start(){
        rb = player.GetComponent<Rigidbody2D>();
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
        float verticalShift = Input.GetAxis("Vertical") * moveShiftMultY + intialOfsetY; //le changement d'ofset par rapport au déplacement vertical
        ofset = new Vector3(horizontalShift, verticalShift,ofset.z);
        }
        #endregion
            
    }

