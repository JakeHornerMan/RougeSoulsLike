using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;
    PlayerAttacking playerAttacking;

    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;

    void Awake(){
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerAttacking = GetComponent<PlayerAttacking>();
    }

    void Start(){
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void Right_lightAttack(){
        playerAttacking.HandleLightAttack(rightWeapon);
    }

    public void Right_heavyAttack(){
        playerAttacking.HandleHeavyAttack(rightWeapon);
    }
}
