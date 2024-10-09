using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Needed to access input system library
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Use to store player input
    private Vector2 playerInput;
    private Vector3 playerVelocity;

    private Rigidbody rigid;

    // Store ref to player's character controller to be used to move.
    // Fill out field in inspector
    [SerializeField] CharacterController controller;

    // Player speed adjustment value. Modify in inspector for quick prototyping.
    [SerializeField] private float playerSpeed = 1.5f;

    // Player rotation speed adjustement. Modify in inspector for quick prototyping.
    [SerializeField] private float playerRotation = 100f;

    [SerializeField] private float gravityValue = -9.81f;

    private Animator animator;

    private void OnMove(InputValue value)
    {
        // store value received from input either keyboard or controller
        playerInput = value.Get<Vector2>();

        float inputMagnitude = playerInput.magnitude;
        animator.SetFloat("Speed", inputMagnitude);
    }

    private void PlayerMovement()
    {

        // Use transform.rotate API. Use the declaration that uses axis of rotation
        // and "X" amount of degrees to rotate around given axis by. We will use
        // transform.up since that is the center axis of our character. For the degrees,
        // take player.x input, multiply it by rotation speed and time.deltaTime to smooth.
        transform.Rotate(0, playerInput.x * playerRotation * Time.deltaTime, 0);

        Vector3 move = transform.forward * playerInput.y;

        // Use characxter controller API to move player. transform.forward
        // Gives player's facing direction, which is multiplied by speed,
        // and time.deltatime to make it smoother, and playinput.y to only
        // allow for forward and backward movement.
        controller.Move(move * playerSpeed * Time.deltaTime);

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        bool isMoving = playerInput.magnitude > 0;
        animator.SetBool("IsWalking", isMoving);

    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
}
