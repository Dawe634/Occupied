using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
public float speed = 0.5f;

public CharacterController myController;

public Transform myCameraHead;

public float mouseSensitivity = 750f;

private float cameraVerticalRotation;

public GameObject bullet;

public Transform firePosition;

public GameObject muzzleFlash, bulletHole, goopSpray;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        CameraMovement();

        Shoot();
    }

    private void Shoot()
    {
       if(Input.GetMouseButtonDown(0))
        {
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


                
            }
            else
            {
                firePosition.LookAt(myCameraHead.position + (myCameraHead.forward * 50f));
            }

            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);
            Instantiate(bullet, firePosition.position, firePosition.rotation);


        }

    }

    private void CameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraVerticalRotation -= +mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);

        myCameraHead.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }

    void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = x * transform.right + z * transform.forward;
        movement = movement * speed * Time.deltaTime;

        myController.Move(movement);
    }

}
