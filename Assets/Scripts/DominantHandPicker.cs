using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominantHandPicker : MonoBehaviour
{
    //public GameObject leftController;
    //public GameObject rightController;

    public GameObject leftRealHandPosition;
    public GameObject rightRealHandPosition;

    public GameObject leftVirtualHandPosition;
    public GameObject rightVirtualHandPosition;

    public GameObject leftHandIK;
    public GameObject rightHandIK;

    //public GameObject leftHandAvatar;
    //public GameObject rightHandAvatar;

    public GameObject leftRealShoulderPosition;
    public GameObject rightRealShoulderPosition;

    public GameObject leftVirtualShoulderPosition;
    public GameObject rightVirtualShoulderPosition;

    //public GameObject leftVirtualWristAvatar;
    //public GameObject rightVirtualWristAvatar;

    public GameObject leftRealElbowPosition;
    public GameObject rightRealElbowPosition;

    public GameObject leftVirtualElbowPosition;
    public GameObject rightVirtualElbowPosition;

    public GameObject leftElbowIK;
    public GameObject rightElbowIK;


    //public GameObject Controller { get; private set; }

    public GameObject RealHandPosition { get; private set; }

    public GameObject VirtualHandPosition { get; private set; }

    public GameObject HandIK { get; private set; }

    //public GameObject HandAvatar { get; private set; }

    //public GameObject VirtualWristAvatar { get; private set; }

    public GameObject RealElbowPosition { get; private set; }

    public GameObject VirtualElbowPosition { get; private set; }

    public GameObject ElbowIK { get; private set; }

    public GameObject RealShoulderPosition { get; private set; }
    public GameObject VirtualShoulderPosition { get; private set; }

    public EDominantHand dominantHand = EDominantHand.Right;

    public static DominantHandPicker Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject original = GameObject.FindGameObjectWithTag("Original");
        leftRealHandPosition = original.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.L/upper_arm.L/forearm.L/hand.L").gameObject;
        leftRealElbowPosition = original.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.L/upper_arm.L/forearm.L").gameObject;
        leftRealShoulderPosition = original.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.L/upper_arm.L").gameObject;

        rightRealHandPosition = original.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.R/upper_arm.R/forearm.R/hand.R").gameObject;
        rightRealElbowPosition = original.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.R/upper_arm.R/forearm.R").gameObject;
        rightRealShoulderPosition = original.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.R/upper_arm.R").gameObject;

        GameObject avatar = GameObject.FindGameObjectWithTag("Avatar");
        leftVirtualHandPosition = avatar.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.L/upper_arm.L/forearm.L/hand.L").gameObject;
        leftVirtualElbowPosition = avatar.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.L/upper_arm.L/forearm.L").gameObject;
        leftVirtualShoulderPosition = avatar.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.L/upper_arm.L").gameObject;

        rightVirtualHandPosition = avatar.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.R/upper_arm.R/forearm.R/hand.R").gameObject;
        rightVirtualElbowPosition = avatar.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.R/upper_arm.R/forearm.R").gameObject;
        rightVirtualShoulderPosition = avatar.transform.Find("Unity compliant skeleton/hips/spine/chest/chest1/shoulder.R/upper_arm.R").gameObject;



        if (TargetsCoordinatesCSVReader.Instance is not null)
            dominantHand = TargetsCoordinatesCSVReader.Instance.coordinatesList.coordinates[0].dominantHand == 1 ? EDominantHand.Right : EDominantHand.Left;

        switch (dominantHand)
        {
            case EDominantHand.Left:
                //HandAvatar = leftHandAvatar;
                //VirtualWristAvatar = leftVirtualWristAvatar;
                RealElbowPosition = leftRealElbowPosition;
                VirtualElbowPosition = leftVirtualElbowPosition;
                ElbowIK = leftElbowIK;
                RealShoulderPosition = leftRealShoulderPosition;
                VirtualShoulderPosition = leftVirtualShoulderPosition;
                //Controller = leftController;
                RealHandPosition = leftRealHandPosition;
                VirtualHandPosition = leftVirtualHandPosition;
                HandIK = leftHandIK;
                break;
            case EDominantHand.Right:
                //HandAvatar = rightHandAvatar;
                //VirtualWristAvatar = rightVirtualWristAvatar;
                RealElbowPosition = rightRealElbowPosition;
                VirtualElbowPosition = rightVirtualElbowPosition;
                ElbowIK = rightElbowIK;
                RealShoulderPosition = rightRealShoulderPosition;
                VirtualShoulderPosition = rightVirtualShoulderPosition;
                //Controller = rightController;
                RealHandPosition = rightRealHandPosition;
                VirtualHandPosition = rightVirtualHandPosition;
                HandIK = rightHandIK;
                break;
        }
    }
}