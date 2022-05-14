using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    PlayerAnimationHandler animHandler;
    PlayerManager playerManager;

    private void Start(){
        animHandler = GetComponent<PlayerAnimationHandler>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleLightAttack(WeaponItem weapon){
        if(playerManager.isInteracting){return;}
        animHandler.PlayTargetAnimation(weapon.lightAttack, true);
    }

    public void HandleHeavyAttack(WeaponItem weapon){
        if(playerManager.isInteracting){return;}
        animHandler.PlayTargetAnimation(weapon.heavyAttack, true);
    }
}
