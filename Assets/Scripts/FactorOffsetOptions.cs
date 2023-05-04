using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorOffsetOptions : MonoBehaviour
{
    public GameObject OffsetHand;
    public GameObject OffsetElbow;

    public GameObject centerTargetHand;
    public GameObject centerTargetElbow;

    public GameObject VirtualElbowPosition;
    public GameObject RealElbowPosition;

    public GameObject currentTargetHandSelected { get; set; }
    public GameObject currentTargetElbowSelected { get; set; }

    [Range(-5f, 5f)]
    public float factor = 0.0f;

    private GameObject VirtualHandPosition;
    private GameObject RealHandPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (DominantHandPicker.Instance is not null)
        {
            VirtualHandPosition = DominantHandPicker.Instance.VirtualHandPosition;
            RealHandPosition = DominantHandPicker.Instance.RealHandPosition;
        }
        else
        {
            Debug.LogError("DominantHandPicker is null!");
        }
    }

    void setFactorOffsets()
    {
        float distance = Vector3.Distance(RealHandPosition.transform.position, centerTargetHand.transform.position);
        Vector3 direction = (RealHandPosition.transform.position - centerTargetHand.transform.position).normalized;

        OffsetHand.transform.position = RealHandPosition.transform.position + direction * factor * distance;

        distance = Vector3.Distance(RealElbowPosition.transform.position, centerTargetElbow.transform.position);
        direction = (RealElbowPosition.transform.position - centerTargetElbow.transform.position).normalized;

        OffsetElbow.transform.position = RealElbowPosition.transform.position + direction * factor * distance;

        VirtualHandPosition.transform.position = OffsetHand.transform.position;
        VirtualElbowPosition.transform.position = OffsetElbow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        setFactorOffsets();
    }
}
