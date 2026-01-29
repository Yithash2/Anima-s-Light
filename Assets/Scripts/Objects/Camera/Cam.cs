using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    #region Input
    PlayerStateMachine player;
    [field: SerializeField] public Vector3 offset { get; private set; }
    [SerializeField] float smoothSpeed = 0.125f;  // Smooth speed for transition
    [SerializeField] float sideOffset;
    [SerializeField] Animator animator;

    float _playerSide;
    public Animator CamAnimator { get { return animator; } }

    float _smoothSpeed;


    [SerializeField] bool _isLerping;

    public bool IsFalling = false;

    public bool IsDiving = false;

    #endregion

    // Camera boundaries in screen space (0 to 1 for normalized coordinates)
    [SerializeField] float leftBound = 0.2f;   // Left boundary (as a fraction of screen width)
    [SerializeField] float rightBound = 0.8f;  // Right boundary (as a fraction of screen width)

    [SerializeField] float _innerLeftBound = 0.4f;   // Left boundary (as a fraction of screen width) for middle area
    [SerializeField] float _innerRightBound = 0.6f;  // Right boundary (as a fraction of screen width) for middle area

    void Start()
    {
        _smoothSpeed = smoothSpeed;
        player = PlayerStateMachine.Instance;
        _playerSide = -player.transform.localScale.x;
    }

    void FixedUpdate()
    {
        #region Smooth movement
        // Determine the desired position (based on player position and the offset)
        Vector3 moveDirection = Mathf.Abs(player.Rb.linearVelocity.x) > 1 ? new Vector3(Mathf.Sign(player.Rb.linearVelocity.x),0,0): Vector3.zero;
        Vector3 desiredPosition = player.transform.position + offset + moveDirection * sideOffset;

        // Get the screen position of the player (without offset for bounds calculation)
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(player.transform.position + moveDirection * sideOffset);

        // Get the screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Convert the screen bounds from fraction (0 to 1) to pixel coordinates
        float leftBoundPx = leftBound * screenWidth;
        float rightBoundPx = rightBound * screenWidth;

        // Check if the player is outside the screen bounds in screen space
        bool isPlayerOutsideScreen = playerScreenPosition.x < leftBoundPx || playerScreenPosition.x > rightBoundPx;

        // Smoothly follow the player and check if lerping should be active
        if (isPlayerOutsideScreen || _isLerping)
        {
            // Lerp the camera position to the desired position (smooth transition)
            Vector3 targetPosition = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            if (!_isLerping) _isLerping = true;

            // Check if the player is within the horizontal middle region of the screen
            float innerLeftBoundPx = _innerLeftBound * screenWidth;
            float innerRightBoundPx = _innerRightBound * screenWidth;

            // Stop lerping once the player is within the middle horizontal bounds
            if (playerScreenPosition.x >= innerLeftBoundPx && playerScreenPosition.x <= innerRightBoundPx)
            {
                _isLerping = false;  // Stop lerping once the player is within the middle horizontal bounds
            }
        }
        #endregion

        // Always smoothly follow the player's vertical position
        if (!IsFalling)
        {
            _smoothSpeed = smoothSpeed;
            
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, desiredPosition.y, smoothSpeed), transform.position.z);
        }else
        {
            _smoothSpeed = Mathf.Lerp(_smoothSpeed, 1, 8 * Time.deltaTime);
             
            transform.position = new Vector3(transform.position.x , Mathf.Lerp(transform.position.y, desiredPosition.y, _smoothSpeed), transform.position.z);
        }
        
        if (IsDiving)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10), 3 * Time.deltaTime);
        }
        
    }



    public void ResetPostion()
    {
        _isLerping = true;
    }

    public void ChangeOffset( float y)
    {
        if (offset.y == y) return;

        offset = new Vector3(offset.x, y, offset.z);
    }

}
