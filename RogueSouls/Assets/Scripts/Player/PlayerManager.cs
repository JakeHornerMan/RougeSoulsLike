using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraHandler cameraHandler;
    PlayerMovement playerMovement;
    PlayerLocomotion playerLocomotion;
    PlayerAnimationHandler playerAnimationHandler;

    public bool isInteracting;

    float delta;

    public void Start()
    {
        //animator = GetComponentInChildren<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        delta = Time.deltaTime;

        //old system
        //playerLocomotion.HandleAllLocomotion(inputManager.verticalInput, inputManager.horizontalInput ,delta);

        //New System (no more tutorial hell!)
        playerMovement.HandlePlayerMovement(inputManager.movementInput);
        isInteracting = playerAnimationHandler.animator.GetBool("isInteracting");
        playerMovement.isInteracting = isInteracting;
    }

    private void LateUpdate() 
    {
        cameraHandler.HandleAllCameraMovement();

        //isInteracting = playerAnimationHandler.animator.GetBool("isInteracting");
        
        playerAnimationHandler.animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}
