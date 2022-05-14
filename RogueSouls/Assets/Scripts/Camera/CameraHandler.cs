using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    InputManager inputManager;

    Transform playerTransform;
    Transform cameraPivotTransform;
    Transform cameraTransform;
    
    public LayerMask collisionLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 0.2f;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed =.3f;
    public float cameraPivotSpeed =.3f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float minimumPivot = -35;
    public float maximumPivot = 35;

    public float cameraSphereRadius = 0.2f;

    private void Awake()
    {
        cameraPivotTransform = GameObject.Find("CameraPivot").transform;
        playerTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
        inputManager = FindObjectOfType<InputManager>();
    }

    public void HandleAllCameraMovement(float delta)
    {
        FollowTarget(delta);
        RotateCamera(delta);
        HandleCameraCollisions(delta);
    }

    private void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, playerTransform.position, ref cameraFollowVelocity, delta / cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera(float delta)
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed) / delta;
        pivotAngle = pivotAngle + (inputManager.cameraInputY * cameraPivotSpeed) / delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

        rotation = Vector3.zero; 
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = -pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivotTransform.transform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition),
                collisionLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffset);
        }

        if(Mathf.Abs(targetPosition) < minimumCollisionOffset){
            targetPosition = targetPosition -minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta/0.2f);
        cameraTransform.localPosition = cameraVectorPosition;

    }
}