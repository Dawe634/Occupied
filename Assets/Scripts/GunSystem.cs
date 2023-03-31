using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{

    public Transform myCameraHead;
    public GameObject bullet;
    public Transform firePosition;
    public GameObject muzzleFlash, bulletHole, goopSpray;

    public bool canAutoFire;
    private bool shooting, readyToShoot = true;

    public float timeBetweenShots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (canAutoFire) { shooting = Input.GetMouseButton(0); }
        else { shooting = Input.GetMouseButtonDown(0); }


        if (shooting && readyToShoot)
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

            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);
            Instantiate(bullet, firePosition.position, firePosition.rotation, firePosition);


            StartCoroutine(ResetShot());
        }

    }

    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        readyToShoot = true;
    }
}
