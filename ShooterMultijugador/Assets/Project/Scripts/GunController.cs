using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    #region Variables
    public PlayerController playerController;
    public Gun[] guns;
    public Gun actualGun;
    public int indexGun = 0;
    public int maxGuns = 3;

    float lastShootTime = 0;
    Vector3 currentRotation;
    Vector3 targetRotation;
    public float returnSpeed;
    public float snappienes;

    //recarga de arma
    float lastReload;
    bool reloading;

    //cambio de arma
    float lastChangeTime;
    float chandeTime = 0.3f;
    bool isChangin;
  

    public GameObject prefBulletHole;
    #endregion

    #region Unity Functions
    private void Update()
    {
        if (actualGun != null)
        {
            if (lastShootTime <= 0 && !reloading)
            {
                if (!actualGun.data.automatic)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        if (actualGun.data.actualAmmo > 0)
                        {
                            Shoot();
                        }
                    }
                }
                else
                {
                    if (Input.GetButton("Fire1"))
                    {
                        
                        if (actualGun.data.actualAmmo > 0)
                        {
                            Shoot();
                        }
                    }
                }
            }

            if (Input.GetButtonDown("Reload") && !reloading)
            {
                if (actualGun.data.actualAmmo < actualGun.data.maxAmmoCount)
                {
                    lastReload = 0;
                    reloading = true;
                }
            }

            if (reloading)
            {
                lastReload += Time.deltaTime;
                if (lastReload >= actualGun.data.reloadTime)
                {
                    reloading = false;
                    lastReload = 0;
                    Reload();
                }
            }
        }

        if (Input.GetButtonDown("Gun1") && !reloading )
        {
           
            if (indexGun != 0)
            {
                indexGun = 0;
                lastChangeTime = 0;
                if (actualGun != null)
                {
                    actualGun.gameObject.SetActive(false);
                    actualGun = null;
                }
                isChangin = true;
            }
        }

        if (Input.GetButtonDown("Gun2") && !reloading)
        {
            if (indexGun != 1)
            {

                indexGun = 1;
                lastChangeTime = 0;
                if (actualGun != null)
                {
                    actualGun.gameObject.SetActive(false);
                    actualGun = null;
                }
                isChangin = true;
            }
        }
        if (Input.GetButtonDown("Gun3") && !reloading)
        {
            if (indexGun != 2)
            {

                indexGun = 2;
                lastChangeTime = 0;
                if (actualGun != null)
                {
                    actualGun.gameObject.SetActive(false);
                    actualGun = null;
                }

                isChangin = true;
            }
        }

        if (lastShootTime >= 0)
        {
            lastShootTime -= Time.deltaTime;
        }

        if (isChangin)
        {
            lastChangeTime += Time.deltaTime;
            if (lastChangeTime >= chandeTime)
            {
                isChangin = false;
                ChageGun(indexGun);
            }
        }


        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappienes * Time.deltaTime);
        playerController.recoil.localRotation = Quaternion.Euler(currentRotation);

    }
    #endregion

    #region Custom Funtions
    private void Shoot()
    {
        if (Physics.Raycast(playerController.cam.transform.position, playerController.cam.transform.forward, out RaycastHit hit, actualGun.data.range))
        {
            if (hit.transform != null)
            {
                Debug.Log($"We shootin at {hit.transform.name}");
                GameObject go = Instantiate(prefBulletHole, hit.point + hit.normal * .05f, Quaternion.LookRotation(hit.normal, Vector3.up));
            }
        }
        actualGun.data.actualAmmo--;
        lastShootTime = actualGun.data.fireRate;
        Addrecoil();
    }

    void Addrecoil()
    {
        targetRotation += new Vector3(-actualGun.data.recoil.x, Random.Range(-actualGun.data.recoil.y, actualGun.data.recoil.y), 0f);
    }

    void Reload()
    {
        actualGun.data.actualAmmo = actualGun.data.maxAmmoCount;
    }

    void ChageGun(int index)
    {
        if (guns[index] != null)
        {
            actualGun = guns[index];
            actualGun.gameObject.SetActive(true);
        }
    }


    #endregion
}
