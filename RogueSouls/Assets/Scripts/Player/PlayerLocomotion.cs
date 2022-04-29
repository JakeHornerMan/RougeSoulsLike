using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private IEnumerator coroutine;

    PlayerManager playerManager;
    PlayerAnimationHandler animationHandler;
    InputManager inputManager;
    
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;


    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;

    [Header("Movement Stats")]
    [SerializeField]
    public float walkingSpeed = 2;
    [SerializeField]
    public float movementSpeed = 5;
    [SerializeField]
    public float sprintingSpeed = 8;
    [SerializeField]
    public float rotationSpeed = 10;

    [Header("Jump Stats")]
    public float gravityIntensity = -15;
    public float jumpHeight = 3;

    [Header("Falling Stats")]
    public float inAirTimer;
    public float leapingVelocity = 3f;
    public float fallingVelocity = 33f;
    public float raycastLength = 0.2f;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;
    
    public void Start(){
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        animationHandler = GetComponent<PlayerAnimationHandler>();

        cameraObject = Camera.main.transform;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void HandleAllLocomotion(float vertical, float horizontal, float delta)
    {
        HandleFallingAndLanding();

        if(playerManager.isInteracting){
            return;
        }
        if(isJumping){ 
            return; 
        }

        HandleMovement(vertical, horizontal);
        HandleRotation(vertical, horizontal);
    }

    private void HandleMovement(float vertical, float horizontal)
    {
        if(isJumping){ return; }

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
        if(isJumping){ return; }

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

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position; 
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        if(!isGrounded && !isJumping){
            if(!playerManager.isInteracting){
                animationHandler.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if(Physics.SphereCast(rayCastOrigin, raycastLength, -Vector3.up, out hit, groundLayer)){
            if(!isGrounded && !playerManager.isInteracting){
                animationHandler.PlayTargetAnimation("Landing", true);
            }
            inAirTimer = 0;
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }
    }

    public void HandleJump(){
        coroutine = HandleJumping(0.2f);
        StartCoroutine(coroutine);
    }
    private IEnumerator HandleJumping(float waitTime){
        
        if(isGrounded){
            animationHandler.animator.SetBool("isJumping",true);
            animationHandler.PlayTargetAnimation("Jumping", false);

            float jumpingVelocity = Mathf.Sqrt(-1 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;

            yield return new WaitForSeconds(waitTime);

            playerRigidbody.velocity = playerVelocity;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 rayCastOrigin = transform.position; 
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rayCastOrigin, raycastLength);
    }
}
