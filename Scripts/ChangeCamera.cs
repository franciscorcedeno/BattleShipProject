using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public float cameraAngularVelocity = 60f;
    public float cameraDistance = 9f;
    public float cameraAngleY = 0;
    public float cameraAngleX = 62;

    private Camera mainCam;
    bool isLookingAtPlayerBoard = true;
    Vector3 setLocation = new Vector3(5f, 0, 5f);

    void Start()
    {
        mainCam = Camera.main;
        
    }

    void Update()
    {
        float angleDelta = cameraAngularVelocity * Time.deltaTime;

        //Standard Input management
        if (Input.GetKey("up"))
        {
            cameraAngleX += angleDelta;
        }
        if (Input.GetKey("down"))
        {
            cameraAngleX -= angleDelta;
        }
        if (Input.GetKey("right"))
        {
            cameraAngleY -= angleDelta;
        }
        if (Input.GetKey("left"))
        {
            cameraAngleY += angleDelta;
        }

        //Protections
        cameraAngleX = Mathf.Clamp(cameraAngleX, -90f, 90f);
        cameraAngleY = Mathf.Repeat(cameraAngleY, 360f);

        Quaternion cameraRotation =
            Quaternion.AngleAxis(cameraAngleY, Vector3.up)
            * Quaternion.AngleAxis(cameraAngleX, Vector3.right);

        Vector3 cameraPosition =
           setLocation + cameraRotation * Vector3.back * cameraDistance;

        mainCam.transform.position = cameraPosition;
        mainCam.transform.rotation = cameraRotation;

    }

    public void SwitchCamera()
    {
        UnityEngine.Debug.Log("Switch Cmaera has been called!");
        if (isLookingAtPlayerBoard == true)
        {
            UnityEngine.Debug.Log("switching to enemyBoard");
            isLookingAtPlayerBoard = false;
            //Vector3 cameraPosition = new Vector3(-5f, 0, -5f);
            //transform.position = cameraPosition;
            setLocation = new Vector3(-5f, 0, 5f);
            this.transform.position = setLocation;
        }
        else
        {
            UnityEngine.Debug.Log("switching to playerboard");
            isLookingAtPlayerBoard = true;
            setLocation = new Vector3(5f, 0, 5f);
            this.transform.position = setLocation;
        }
    }

}
