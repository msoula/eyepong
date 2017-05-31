﻿using UnityEngine;
using System.Collections;

public class CamShakeSimple : MonoBehaviour
{

    Vector3 originalCameraPosition;

    float shakeAmt = 0;

    public Camera mainCamera;

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.name == "wall_left" || col.gameObject.name == "wall_right") {
            originalCameraPosition = mainCamera.transform.position;
            shakeAmt = col.relativeVelocity.magnitude * .0025f;
            InvokeRepeating("CameraShake", 0, .01f);
            Invoke("StopShaking", 0.3f);
        }

    }

    void CameraShake()
    {
        if (shakeAmt>0)
        {
            float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
            Vector3 pp = mainCamera.transform.position;
            pp.x+= quakeAmt; // can also add to x and/or z
            pp.y+= quakeAmt; // can also add to x and/or z
            mainCamera.transform.position = pp;
        }
    }

    void StopShaking()
    {
        CancelInvoke("CameraShake");
        mainCamera.transform.position = originalCameraPosition;
    }

}