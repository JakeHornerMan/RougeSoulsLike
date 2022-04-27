using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    PlayerAnimationHandler playerAnimationHandler;

    public bool isInteracting;

    float delta;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraHandler = FindObjectOfType<CameraHandler>();
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

        playerLocomotion.HandleAllLocomotion(inputManager.verticalInput, inputManager.horizontalInput ,delta);
    }

    private void LateUpdate() 
    {
        cameraHandler.HandleAllCameraMovement();

        //isInteracting = 
    }
}
