using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCalibration : MonoBehaviour
{
    public GameObject TargetsHand;
    public GameObject TargetsElbow;

    public bool initCalibrationDone { set; get; }
    public GameObject RealLeftHand;
    public GameObject RealRightHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FollowHeadsetYPosition()
    {
        //transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, 1.0f);

        foreach (Transform child in TargetsHand.transform)
            child.position = new Vector3(child.position.x, Camera.main.transform.position.y - 0.3f, child.position.z);
    }

    /// <summary>
    /// This method of calibration sets the position and rotation of the TargetsHand parent object to match the dominant real hand controller.
    /// </summary>
    void InitialCalibration_V1()
    {
        if (OVRManager.instance)
            OVRManager.display.RecenterPose();

        if(DominantHandPicker.Instance is not null)
        {
            switch (DominantHandPicker.Instance.dominantHand)
            {
                case EDominantHand.Right:
                    TargetsHand.transform.position = RealRightHand.transform.position;
                    TargetsHand.transform.rotation = Quaternion.Euler(0, RealRightHand.transform.eulerAngles.y + 90, 0);
                    break;
                case EDominantHand.Left:
                    TargetsHand.transform.position = RealLeftHand.transform.position;
                    TargetsHand.transform.rotation = Quaternion.Euler(0, RealLeftHand.transform.eulerAngles.y + 90, 180);
                    break;
            }
        }
        else
        {
            // Default to right dominant hand
            TargetsHand.transform.position = RealRightHand.transform.position;
            TargetsHand.transform.rotation = Quaternion.Euler(0, RealRightHand.transform.eulerAngles.y, 0);
        }
        initCalibrationDone = true;
    }

    /// <summary>
    /// This method of calibration puts the targets at +/- 30 degrees of flexion/extension for elbow and shoulder.
    /// <see link="visualisation_combinaison.xlsx"/>
    /// </summary>
    void InitialCalibration_V2()
    {
        // Get Targets position to origin for easier calculations of targets
        transform.position = Vector3.zero;

        TargetsCoordinatesCSVReader csv = TargetsCoordinatesCSVReader.Instance;
        (float x, float z) coordinates;
        float coordinateY = 0;

        foreach (Transform child in TargetsElbow.transform)
        {
            switch (child.name)
            {
                case string name when name == ETargetElbow.R.ToString():
                    coordinates = csv.getCoordinates(EArmPosition.pointR.ToString(), EBodyPart.elbow.ToString());
                    child.position = new Vector3(coordinates.x, coordinateY, coordinates.z);
                    break;
                case string name when name == ETargetElbow.PM_PP.ToString():
                    coordinates = csv.getCoordinates(EArmPosition.pointPP.ToString(), EBodyPart.elbow.ToString());
                    child.position = new Vector3(coordinates.x, coordinateY, coordinates.z);
                    break;
                case string name when name == ETargetElbow.MM_MP.ToString():
                    coordinates = csv.getCoordinates(EArmPosition.pointMM.ToString(), EBodyPart.elbow.ToString());
                    child.position = new Vector3(coordinates.x, coordinateY, coordinates.z);
                    break;
            }
        }

        foreach (Transform child in TargetsHand.transform)
        {
            switch (child.name)
            {
                case string name when name == ETargetHand.R.ToString():
                    coordinates = csv.getCoordinates(EArmPosition.pointR.ToString(), EBodyPart.hand.ToString());
                    child.position = new Vector3(coordinates.x, coordinateY, coordinates.z);
                    break;
                case string name when name == ETargetHand.PP.ToString():
                    coordinates = csv.getCoordinates(EArmPosition.pointPP.ToString(), EBodyPart.hand.ToString());
                    child.position = new Vector3(coordinates.x, coordinateY, coordinates.z);
                    break;
                case string name when name == ETargetHand.MM.ToString():
                    coordinates = csv.getCoordinates(EArmPosition.pointMM.ToString(), EBodyPart.hand.ToString());
                    child.position = new Vector3(coordinates.x, coordinateY, coordinates.z);
                    break;
                case string name when name == ETargetHand.MP.ToString():
                    coordinates = csv.getCoordinates(EArmPosition.pointMP.ToString(), EBodyPart.hand.ToString());
                    child.position = new Vector3(coordinates.x, coordinateY, coordinates.z);
                    break;
                case string name when name == ETargetHand.PM.ToString():
                    coordinates = csv.getCoordinates(EArmPosition.pointPM.ToString(), EBodyPart.hand.ToString());
                    child.position = new Vector3(coordinates.x, coordinateY, coordinates.z);
                    break;
            }
        }
        // TODO: Set this position as the shoulder position determined by Vicon
        transform.position = DominantHandPicker.Instance.Shoulder.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            InitialCalibration_V2();
        }
    }
}
