using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidBody;
    [SerializeField] float _speed;

    Vector2 Input;
    public void Move(InputAction.CallbackContext ctx)
    {
        Input = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        var move = new Vector3(Input.x, 0 ,Input.y).normalized;

        _rigidBody.velocity = move * _speed;
    }
}
