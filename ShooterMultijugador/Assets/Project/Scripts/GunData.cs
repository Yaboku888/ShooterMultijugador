using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class GunData : ScriptableObject
{
    #region Variables
    public string gunName;
    public int maxAmmoCount;
    public int actualAmmo;
    public float reloadTime;
    public float damage;
    public float fireRate;
    public float range;
    public Vector2 recoil;

    public bool automatic;
    #endregion
}
