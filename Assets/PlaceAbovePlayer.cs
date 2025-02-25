using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAbovePlayer : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector3(PlayerStateMachine.Instance.transform.position.x, transform.position.y, 0);
    }
}
