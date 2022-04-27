using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    public bool isSprinting;

    [Header("Movement Stats")]
    [SerializeField]
    public float walkingSpeed = 2;
    [SerializeField]
    public float movementSpeed = 5;
    [SerializeField]
    public float sprintingSpeed = 8;
    [SerializeField]
    public float rotationSpeed = 10;
    
    public void Start(){
        inputManager = GetComponent<InputManager>();

        cameraObject = Camera.main.transform;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void HandleAllLocomotion(float vertical, float horizontal, float delta)
    {
        HandleMovement(vertical, horizontal);
        HandleRotation(vertical, horizontal);
    }

    private void HandleMovement(float vertical, float horizontal)
    {

        moveDirection = cameraObject.forward * vertical;
        moveDirection = moveDirection + cameraObject.right * horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if(isSprinting){
            moveDirection = moveDirection * sprintingSpeed;
        }
        else{
            if(inputManager.moveAmount >= 0.5f){
            moveDirection = moveDirection * movementSpeed;
            }
            else{
                moveDirection = moveDirection * walkingSpeed;
            }
        }
        
        
        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;

    }

    private void HandleRotation(float vertical, float horizontal)
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * vertical;
        targetDirection = targetDirection + cameraObject.right * horizontal;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero){
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        transform.rotation = playerRotation;
    }
}
