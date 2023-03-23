using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominantHandPicker : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController;

    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject leftShoulder;
    public GameObject rightShoulder;

    public GameObject leftVirtualWrist;
    public GameObject rightVirtualWrist;

    public GameObject leftElbow;
    public GameObject rightElbow;

    public GameObject leftVirtualElbow;
    public GameObject rightVirtualElbow;

    public GameObject Controller { get; private set; }

    public GameObject Hand { get; private set; }

    public GameObject VirtualWrist { get; private set; }

    public GameObject Elbow { get; private set; }

    public GameObject VirtualElbow { get; private set; }

    public GameObject Shoulder { get; private set; }

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
                Hand = leftHand;
                VirtualWrist = leftVirtualWrist;
                Elbow = leftElbow;
                VirtualElbow = leftVirtualElbow;
                Shoulder = leftShoulder;
                Controller = leftController;
                break;
            case EDominantHand.Right:
                Hand = rightHand;
                VirtualWrist = rightVirtualWrist;
                Elbow = rightElbow;
                VirtualElbow = rightVirtualElbow;
                Shoulder = rightShoulder;
                Controller = rightController;
                break;
        }
    }
}