using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecenterCamera : MonoBehaviour
{
    private GameObject tete;
    public Transform steamCamera;
    public float[] offset = new float[3];

    private void Start()
    {
        tete = GameObject.Find("head");        
    }

    private void Update()
    {

    }

    public void CameraCalibration()
    {
        tete = GameObject.Find("head");

        if (tete != null)
        {
            ResetSeatedPos(tete.transform);
        }
        else
        {
            Debug.LogError("Tete Transform not found. Missing tag ?", gameObject);
        }
    }

    private void ResetSeatedPos(Transform desiredHeadPos)
    {
        if ((steamCamera != null) )
        {
            this.transform.rotation = Quaternion.identity;
            this.transform.position = Vector3.zero;
            //ROTATION
           this.transform.rotation = Quaternion.Euler(new Vector3(0f, Quaternion.FromToRotation(steamCamera.forward, Vector3.forward).eulerAngles.y, 0f));

            //POSITION
            // Calculate postional offset between CameraRig and Camera
            Vector3 offsetPos = steamCamera.position - this.transform.position;
            // Reposition CameraRig to desired position minus offset
            Vector3 nextPosition = (desiredHeadPos.position - offsetPos);
            this.transform.position = new Vector3(nextPosition.x + offset[0], nextPosition.y + offset[1], nextPosition.z + offset[2]);

            Debug.Log("Seat recentered!");
        }
        else
        {
            Debug.Log("Error: SteamVR objects not found!");
        }
    }
}
