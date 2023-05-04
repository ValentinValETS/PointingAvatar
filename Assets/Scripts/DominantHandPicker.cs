using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominantHandPicker : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController;

    public GameObject leftRealHandPosition;
    public GameObject rightRealHandPosition;

    public GameObject leftVirtualHandPosition;
    public GameObject rightVirtualHandPosition;

    public GameObject leftHandAvatar;
    public GameObject rightHandAvatar;

    public GameObject leftShoulderAvatar;
    public GameObject rightShoulderAvatar;

    public GameObject leftVirtualWristAvatar;
    public GameObject rightVirtualWristAvatar;

    public GameObject leftElbowAvatar;
    public GameObject rightElbowAvatar;

    public GameObject leftVirtualElbowAvatar;
    public GameObject rightVirtualElbowAvatar;

    public GameObject Controller { get; private set; }

    public GameObject RealHandPosition { get; private set; }

    public GameObject VirtualHandPosition { get; private set; }

    public GameObject HandAvatar { get; private set; }

    public GameObject VirtualWristAvatar { get; private set; }

    public GameObject ElbowAvatar { get; private set; }

    public GameObject VirtualElbowAvatar { get; private set; }

    public GameObject ShoulderAvatar { get; private set; }

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
        if (TargetsCoordinatesCSVReader.Instance is not null)
            dominantHand = TargetsCoordinatesCSVReader.Instance.coordinatesList.coordinates[0].dominantHand == 1 ? EDominantHand.Right : EDominantHand.Left;

        switch (dominantHand)
        {
            case EDominantHand.Left:
                HandAvatar = leftHandAvatar;
                VirtualWristAvatar = leftVirtualWristAvatar;
                ElbowAvatar = leftElbowAvatar;
                VirtualElbowAvatar = leftVirtualElbowAvatar;
                ShoulderAvatar = leftShoulderAvatar;
                Controller = leftController;
                RealHandPosition = leftRealHandPosition;
                VirtualHandPosition = leftVirtualHandPosition;
                break;
            case EDominantHand.Right:
                HandAvatar = rightHandAvatar;
                VirtualWristAvatar = rightVirtualWristAvatar;
                ElbowAvatar = rightElbowAvatar;
                VirtualElbowAvatar = rightVirtualElbowAvatar;
                ShoulderAvatar = rightShoulderAvatar;
                Controller = rightController;
                RealHandPosition = rightRealHandPosition;
                VirtualHandPosition = rightVirtualHandPosition;
                break;
        }
    }
}