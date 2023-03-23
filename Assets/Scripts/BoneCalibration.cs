using DitzelGames.FastIK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Used for avatar that has only one arm. Use BoneCalibration_V2.cs for avatar with 2 arms.", false)]
public class BoneCalibration : MonoBehaviour
{
    public GameObject Hand;
    public GameObject ForeArm;
    public GameObject Controller;
    public GameObject Elbow;

    // Start is called before the first frame update
    void Start()
    {
        activateFastIK(Hand);
    }

    void activateFastIK(GameObject bone)
    {
        bone.GetComponent<FastIKFabric>().enabled = true;
    }

    void deactivateFastIK(GameObject bone)
    {
        bone.GetComponent<FastIKFabric>().enabled = false;
    }

    void calibrateForearmDistance()
    {
        deactivateFastIK(ForeArm);
        deactivateFastIK(Hand);

        float devicesDistance = Vector3.Distance(Controller.transform.position, Elbow.transform.position);
        float boneDistance = Vector3.Distance(Hand.transform.position, ForeArm.transform.position);
        float distanceDifference = devicesDistance - boneDistance;

        Debug.Log(distanceDifference);

        Hand.transform.localPosition = new Vector3(Hand.transform.localPosition.x, Hand.transform.localPosition.y + distanceDifference, Hand.transform.localPosition.z);

        activateFastIK(ForeArm);
        activateFastIK(Hand);
    }

    void calibrateForearmDistance(float value)
    {
        deactivateFastIK(ForeArm);
        deactivateFastIK(Hand);

        Hand.transform.localPosition = new Vector3(Hand.transform.localPosition.x, Hand.transform.localPosition.y + value, Hand.transform.localPosition.z);

        activateFastIK(ForeArm);
        activateFastIK(Hand);
    }

    void scaleArmature(float value)
    {
        transform.localScale = new Vector3(transform.localScale.x + value, transform.localScale.y + value, transform.localScale.z + value);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            calibrateForearmDistance(0.0005f);
            scaleArmature(0.5f);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            calibrateForearmDistance(-0.0005f);
            scaleArmature(-0.5f);
        }
    }
}
