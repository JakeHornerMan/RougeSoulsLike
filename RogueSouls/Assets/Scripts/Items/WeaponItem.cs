using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public int damage;
    public int staminaCost;
    public int ManaCost;
    public bool isUnarmed;
}
