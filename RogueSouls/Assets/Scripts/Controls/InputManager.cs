using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    //PlayerLocomotion playerLocomotion;
    PlayerMovement playerMovement;
    PlayerInventory playerInventory;

    PlayerAnimationHandler playerAnimationHandler;
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    public bool run_input;
    public bool jump_input;
    public bool dodge_input;
    public bool r_lightAttack;
    public bool r_heavyAttack;

    private void Awake(){
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        //playerLocomotion = GetComponent<PlayerLocomotion>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleRollingInput();
        HandleAttackInput();
    }

    private void OnEnable()
    {
        if (playerControls == null){
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Run.performed += i => run_input = true;
            playerControls.PlayerActions.Run.canceled += i => run_input = false;

            playerControls.PlayerActions.Jump.performed += i => jump_input = true;

            playerControls.PlayerActions.Dodge.performed += i => dodge_input = true;
            
            playerControls.Combat.RightLightAttack.performed += i => r_lightAttack = true;
            playerControls.Combat.RightHeavyAttack.performed += i => r_heavyAttack = true;
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
        
        if(playerMovement.isInteracting == false){
            playerAnimationHandler.UpdateAnimatorValues(0, moveAmount, playerMovement.isSprinting);
        }
        // else{
        //     playerAnimationHandler.UpdateAnimatorValues(0, 0, false);
        // }

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
    }

    private void HandleSprintingInput(){
        if (run_input && moveAmount > 0.5f){
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
            //playerMovement.isJumping = true;
            playerMovement.Jump();
        }
    }

    private void HandleRollingInput(){
        if(dodge_input){
            playerMovement.isDodging = true;
            dodge_input = false;
        }
    }

    private void HandleAttackInput(){
        if(r_lightAttack){
            playerInventory.Right_lightAttack();
            r_lightAttack = false;
        }

        if(r_heavyAttack){
            playerInventory.Right_heavyAttack();
            r_heavyAttack = false;
        }
    }
}
