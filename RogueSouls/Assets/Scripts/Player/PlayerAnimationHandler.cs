using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");   
    }

    public void UpdateAnimatorValues(float verticalInput, float horizontalInput, bool isSprinting)
    {
        //animation setting to distinct values
        float v= 0;
        if(verticalInput > 0 && verticalInput < 0.55f){
            v = 0.5f;
        }
        else if (verticalInput  > 0.55f){
            v = 1;
        }
        else if (verticalInput < 0 && verticalInput > -0.55f){
            v = 0.5f;
        }
        else if (verticalInput < -0.55f){
            v = 1;
        }
        else{
            v = 0; 
        }

        float h= 0;
        if(horizontalInput > 0 && horizontalInput < 0.55f){
            h = 0.5f;
        }
        else if (horizontalInput  > 0.55f){
            h = 1;
        }
        else if (horizontalInput < 0 && horizontalInput > -0.55f){
            h = 0.5f;
        }
        else if (horizontalInput < -0.55f){
            h = 1;
        }
        else{
            h = 0; 
        }

        if(isSprinting){
            h = horizontalInput;
            v =2;
        }

        animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation,0.2f);
    }

    public void SetBoolField(string field, bool ans){
        animator.SetBool(field, ans);
    }

    public void JumpAnim(){
        animator.SetBool("isJumping", true);
        PlayTargetAnimation("Jumping", false);
    }
    
}
