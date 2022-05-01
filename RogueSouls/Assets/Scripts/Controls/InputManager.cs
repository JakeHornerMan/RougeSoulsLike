using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    PlayerMovement playerMovement;
    PlayerAnimationHandler playerAnimationHandler;
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    public bool b_input;

    public bool jump_input;

    private void Awake(){
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void OnEnable()
    {
        if (playerControls == null){
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => b_input = true;
            playerControls.PlayerActions.B.canceled += i => b_input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleMovementInput(){
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        
        if(playerMovement.isGrounded){
            playerAnimationHandler.UpdateAnimatorValues(0, moveAmount, playerMovement.isSprinting);
        }
        else{
            playerAnimationHandler.UpdateAnimatorValues(0, 0, false);
        }

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
    }

    private void HandleSprintingInput(){
        if (b_input && moveAmount > 0.5f){
            playerMovement.isSprinting = true;
        }
        else{
            playerMovement.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if(jump_input){
            jump_input = false;
            playerMovement.Jump();
            playerAnimationHandler.SetBoolField("isJumping",true);
        }
    }
}
