using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    #region Input
    GameObject player;
    [field: SerializeField] public Vector3 offset { get; private set; }
    [SerializeField] float smoothSpeed = 0.125f;  // Smooth speed for transition
    [SerializeField] float moveShiftMultX;
    [SerializeField] float moveShiftMultY;
    [SerializeField] Animator animator;

    public Animator CamAnimator { get { return animator; } }

    public float verticalShift;

    [SerializeField] bool _isLerping;

    float initialOffsetY;
    #endregion

    [field: SerializeField] public GameObject Background { get; private set; }

    // Camera boundaries in screen space (0 to 1 for normalized coordinates)
    [SerializeField] float leftBound = 0.2f;   // Left boundary (as a fraction of screen width)
    [SerializeField] float rightBound = 0.8f;  // Right boundary (as a fraction of screen width)

    [SerializeField] float _innerLeftBound = 0.4f;   // Left boundary (as a fraction of screen width) for middle area
    [SerializeField] float _innerRightBound = 0.6f;  // Right boundary (as a fraction of screen width) for middle area

    void Start()
    {
        player = PlayerStateMachine.Instance.gameObject;
        initialOffsetY = offset.y;
    }

    void FixedUpdate()
    {
        #region Smooth movement
        // Determine the desired position (based on player position and the offset)
        Vector3 desiredPosition = player.transform.position + offset;

        // Get the screen position of the player
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(desiredPosition);

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
            // Lerp the camera position to the player's position (smooth transition)
            Vector3 targetPosition = new Vector3(desiredPosition.x, transform.position.y, desiredPosition.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            if (!_isLerping) _isLerping = true;

            // Check if the player is within the horizontal middle region of the screen
            float innerLeftBoundPx = _innerLeftBound * screenWidth;
            float innerRightBoundPx = _innerRightBound * screenWidth;

            Debug.Log($"{playerScreenPosition.x} - {innerLeftBoundPx} : {playerScreenPosition.x} - {innerRightBoundPx}");
            if (playerScreenPosition.x >= innerLeftBoundPx && playerScreenPosition.x <= innerRightBoundPx)
            {
                _isLerping = false;  // Stop lerping once the player is within the middle horizontal bounds
            }
        }
        #endregion

        // Always smoothly follow the player's vertical position
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, desiredPosition.y, smoothSpeed), transform.position.z);

        offset = Vector3.Lerp(offset, new Vector3(offset.x, verticalShift, offset.z), smoothSpeed);
    }

    public void ResetPostion()
    {
        _isLerping = true;
    }

    public void ResetVerticalShift()
    {
        offset = new Vector3(offset.x, 0, offset.z);
    }
}
