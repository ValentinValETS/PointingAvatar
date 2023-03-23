using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRArmMap
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

public class VRArmRig : VRRigParent
{
    public VRArmMap hand;
    public VRArmMap elbow;
    public VRArmMap shoulder;

    // Update is called once per frame
    void LateUpdate()
    {
        hand.Map();
        elbow.Map();
        shoulder.Map();
    }
}
