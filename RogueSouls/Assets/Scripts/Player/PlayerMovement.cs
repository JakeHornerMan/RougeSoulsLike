using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private IEnumerator coroutine;
    Rigidbody playerRigidbody;
    Transform cameraTransform;
    InputManager inputManager;
    PlayerAnimationHandler playerAnimationHandler;
    public LayerMask groundLayer;

    Vector3 moveDirection;

    [Header("Movement Flags")]
    public bool DissableMove = true;
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

    [Header("Gizmo Debug")]
    public float rayCastHeightOffset = 0.25f;
    public float raycastLength = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerAnimationHandler= GetComponent<PlayerAnimationHandler>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    public void HandlePlayerMovement(Vector2 movementInput)
    {
        // if(isJumping && isGrounded){
        //     Jump();
        // }

        //IsGrounded();
        if(!DissableMove){
            if(isGrounded){
                Rotation(movementInput);
                Move(movementInput);
            }
        }  
    }

    public void Move(Vector2 movementInput){

        Vector2 input = new Vector2(movementInput.x, movementInput.y);
        Vector2 inputDirirection = input.normalized;

        if(isJumping){return;}

        moveDirection = cameraTransform.forward * inputDirirection.y;
        moveDirection = moveDirection + cameraTransform.right * inputDirirection.x;
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

    public void Rotation(Vector2 movementInput){
        
        Vector2 input = new Vector2(movementInput.x, movementInput.y);
        Vector2 inputDirirection = input.normalized;

        if(isJumping){return;}

        float targetAngle = Mathf.Atan2(inputDirirection.x, inputDirirection.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Quaternion rotate = Quaternion.Euler(0, targetAngle, 0);
        if (inputDirirection != Vector2.zero){
            playerRigidbody.rotation = 
                Quaternion.Lerp(playerRigidbody.rotation, rotate, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    public void Jump() {
        isJumping = true;

        float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
        Vector3 playerVelocity = moveDirection;
        playerVelocity.y = jumpingVelocity;

        playerAnimationHandler.JumpAnim();

        DisableMovement(.2f);

        playerRigidbody.velocity = playerVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            isGrounded = true;
            isJumping = false;
            DisableMovement(.5f);
            //playerAnimationHandler.SetBoolField("isInteracting", true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 rayCastOrigin = transform.position; 
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rayCastOrigin, raycastLength);
    }

    public void DisableMovement(float time){
        coroutine = WaitForMovement(time);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitForMovement(float time){
        DissableMove = true;
        yield return new WaitForSeconds(time);
        DissableMove = false;
    }
}
