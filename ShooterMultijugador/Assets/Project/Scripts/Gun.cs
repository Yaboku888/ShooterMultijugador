using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Variables
    public GunData data;
    #endregion

    #region Unity Functins
    private void Awake()
    {
        data.actualAmmo = data.maxAmmoCount;
    }
    #endregion
}
