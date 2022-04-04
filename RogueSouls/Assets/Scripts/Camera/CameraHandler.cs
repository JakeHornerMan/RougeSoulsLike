using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    InputManager inputManager;
    Transform targetTransform;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public float cameraFollowSpeed = 0.2f;

    public float lookAngle;
    public float pivotAngle;

    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerManager>().transform;
    }

    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    public void RotateCamera()
    {
        // lookAngle = lookAngle + (mouseX * cameraLookSpeed);
        // pivotAngle = pivotAngle + (mouseY * cameraPivotSpeed);
    }
}

