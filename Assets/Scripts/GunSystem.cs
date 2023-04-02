using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{

    public Transform myCameraHead;
    private UICanvasController myUICanvas;

    public GameObject bullet;
    public Transform firePosition;
    public GameObject muzzleFlash, bulletHole, goopSpray;

    public bool canAutoFire;
    private bool shooting, readyToShoot = true;

    public float timeBetweenShots;

    public int bulletsAvailable;
    public int totalBullets, magazineSize;


    public float reloadTime;
    private bool reloading;


    // Start is called before the first frame update
    void Start()
    {
        totalBullets -= magazineSize;
        bulletsAvailable = magazineSize;

        myUICanvas = FindObjectOfType<UICanvasController>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        GunManager();

        UpdateAmmoText();
    }

    private void GunManager()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsAvailable < magazineSize && !reloading) {
            Reload();
        }
    }

    private void Shoot()
    {
        if (canAutoFire) { shooting = Input.GetMouseButton(0); }
        else { shooting = Input.GetMouseButtonDown(0); }


        if (shooting && readyToShoot && bulletsAvailable > 0 && !reloading)
        {
            readyToShoot = false;
            RaycastHit hit;

            if (Physics.Raycast(myCameraHead.position, myCameraHead.forward, out hit, 100f))
            {
                if (Vector3.Distance(myCameraHead.position, hit.point) > 2f)
                {
                    firePosition.LookAt(hit.point);
                    if (hit.collider.tag == "Shootable")
                    {
                        Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                    }

                    if (hit.collider.tag == "Plane")
                        Instantiate(goopSpray, hit.point, Quaternion.LookRotation(hit.normal));

                }


                if (hit.collider.CompareTag("Enemy"))
                { Destroy(hit.collider.gameObject); }



            }
            else
            {
                firePosition.LookAt(myCameraHead.position + (myCameraHead.forward * 50f));
            }

            bulletsAvailable--;

            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);
            Instantiate(bullet, firePosition.position, firePosition.rotation, firePosition);


            StartCoroutine(ResetShot());


        }

    }

    private void Reload()
    {
        int bulletsToAdd = magazineSize - bulletsAvailable;
        if (totalBullets > bulletsToAdd)
        {
            totalBullets -= bulletsToAdd;
            bulletsAvailable = magazineSize;
        }
        else
        {
            bulletsAvailable += totalBullets;
            totalBullets = 0;
        }

        reloading = true;

        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        readyToShoot = true;
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);

        reloading = false;
    }

    private void UpdateAmmoText()
    {
        myUICanvas.ammoText.SetText(bulletsAvailable + "/" + magazineSize);
        myUICanvas.totalAmmo.SetText(totalBullets.ToString());
    }
}
