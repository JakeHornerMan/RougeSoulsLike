using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRigidbody;
    Transform cameraTransform;
    InputManager inputManager;
    public LayerMask groundLayer;

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

    [Header("Gizmo Debug")]
    public float rayCastHeightOffset = 0.25f;
    public float raycastLength = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    public void HandlePlayerMovement(Vector2 movementInput)
    {
        IsGrounded();
        Vector2 input = new Vector2(movementInput.x, movementInput.y);
        Vector2 inputDirirection = input.normalized;

        if(isGrounded){
            Rotation(inputDirirection);
            Move(inputDirirection);
        }

        // if(isJumping){
        //     Jump();
        // }

    }

    public void Move(Vector2 movementInput){

        if(isJumping){return;}

        Vector3 moveDirection = cameraTransform.forward * movementInput.y;
        moveDirection = moveDirection + cameraTransform.right * movementInput.x;
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

    public void Rotation(Vector2 inputDirirection){
        
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
        Debug.Log("isJumping?");
        playerRigidbody.AddForce(transform.up * jumpHeight);
        //this.transform.Translate(Vector3.up * 1);
    }

    public void IsGrounded(){
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position; 
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        if(Physics.SphereCast(rayCastOrigin, raycastLength, -Vector3.up, out hit, groundLayer)){
            isGrounded = true;
            isJumping = false;
        }
        else{
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
}
