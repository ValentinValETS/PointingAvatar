using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRUpperBodyMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public bool affectRotation;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);

        if (affectRotation)
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRUpperBodyRig : VRRigParent
{
    public VRUpperBodyMap chest;

    public VRUpperBodyMap leftHand;
    public VRUpperBodyMap leftElbow;

    public VRUpperBodyMap rightHand;
    public VRUpperBodyMap rightElbow;

    // Update is called once per frame
    void LateUpdate()
    {
        chest.Map();

        leftHand.Map();
        leftElbow.Map();

        rightHand.Map();
        rightElbow.Map();
    }
}
