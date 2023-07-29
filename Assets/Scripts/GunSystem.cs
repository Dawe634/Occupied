using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{

    public Transform myCameraHead;
    private UICanvasController myUICanvas;
    public Animator myAnimator;

    public GameObject bullet;
    public Transform firePosition;
    public GameObject muzzleFlash, bulletHole, goopSpray, bloodSpray, rocketTrail;

    public bool canAutoFire;
    private bool shooting, readyToShoot = true;

    public float timeBetweenShots;

    public int bulletsAvailable;
    public int totalBullets, magazineSize;


    public float reloadTime;
    private bool reloading;

    //aiming

    public Transform aimPosition;
    private float aimSpeed = 2f;
    private Vector3 gunStartPosition;

    public float zoomAmount;
    public int damageAmount;
    public string gunName;

    public bool rocketLauncher;

    string gunAnimationName;

    // Start is called before the first frame update
    void Start()
    {
        totalBullets -= magazineSize;
        bulletsAvailable = magazineSize;

        myUICanvas = FindObjectOfType<UICanvasController>();

        gunStartPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        GunManager();

        UpdateAmmoText();

        AnimationManager();
    }

    private void AnimationManager()
    {
        switch (gunName)
        {
            case "Pistol":
                gunAnimationName = "Pistol Reload";
                break;

            case "Rifle":
                gunAnimationName = "Rifle Reload";
                break;

            case "Sniper":
                gunAnimationName = "Sniper Reload";
                break;

            case "RocketLauncher":
                gunAnimationName = "Rocket Reload";
                break;

            default:
                break;


         }   
    }

    private void GunManager()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsAvailable < magazineSize && !reloading) {
            Reload();
        }

        if (Input.GetMouseButton(1))
        {
            transform.position = Vector3.MoveTowards(transform.position, aimPosition.position, aimSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, gunStartPosition, aimSpeed * Time.deltaTime);
        }

        if(Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<CameraMove>().ZoomIn(zoomAmount);
        }
        if (Input.GetMouseButtonUp(1))
        {
            FindObjectOfType<CameraMove>().ZoomOut();
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

                    if (!rocketLauncher)
                    {
                        if (hit.collider.tag == "Shootable")
                            Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                        if (hit.collider.tag == "Plane")
                            Instantiate(goopSpray, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }


                if (hit.collider.CompareTag("Enemy") && !rocketLauncher)
                { hit.collider.GetComponent<EnemyHealthSystem>().TakeDamage(damageAmount);
                    Instantiate(bloodSpray, hit.point, Quaternion.LookRotation(hit.normal)); }



            }
            else
            {
                
                firePosition.LookAt(myCameraHead.position + (myCameraHead.forward * 50f));
            }

            bulletsAvailable--;

            if (!rocketLauncher)
            {
                Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);
                Instantiate(bullet, firePosition.position, firePosition.rotation, firePosition);
            }
            else
            {
                Instantiate(bullet, firePosition.position, firePosition.rotation);
                Instantiate(rocketTrail, firePosition.position, firePosition.rotation);
            }

            StartCoroutine(ResetShot());
        }

    }

    private void Reload()
    {
        myAnimator.SetTrigger(gunAnimationName);

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

      
    }

    private void UpdateAmmoText()
    {
        myUICanvas.ammoText.SetText(bulletsAvailable + "/" + magazineSize);
        myUICanvas.totalAmmo.SetText(totalBullets.ToString());
    }
}
