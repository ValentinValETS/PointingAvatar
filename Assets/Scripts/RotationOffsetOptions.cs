using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Obsolete("Old method to set offsets located on bones or controller. Use FactorOffsetOptions.cs instead for offsets located bones.", false)]
public class RotationOffsetOptions : MonoBehaviour
{
    public GameObject TargetsHand;
    public GameObject TargetsElbow;
    public GameObject TargetShoulder;

    public GameObject OffsetHand;
    public GameObject OffsetElbow;

    public GameObject centerTargetHand;
    public GameObject centerTargetElbow;

    public GameObject VirtualElbowPosition;
    public GameObject RealElbowPosition;

    public GameObject currentTargetHandSelected { get; set; }
    public GameObject currentTargetElbowSelected { get; set; }

    private GameObject VirtualHandPosition;
    private GameObject RealHandPosition;

    private ExperimentalTrial experimentalTrial = null;

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

    // Update is called once per frame
    void Update()
    {
        if (currentTargetHandSelected is not null && currentTargetElbowSelected is not null && experimentalTrial is not null && experimentalTrial.movementOffset is not EMovementOffset.Congruent)
        {
            // With this code, the offset depends only on the distance travelled from the center, so the more it gets further from the center, lerp tends to 1
            float targetDistanceFromCenter = Vector3.Distance(currentTargetHandSelected.transform.position, centerTargetHand.transform.position);
            float distanceTraveled = Vector3.Distance(RealHandPosition.transform.position, centerTargetHand.transform.position);

            float lerpValue = Mathf.Clamp01(distanceTraveled / targetDistanceFromCenter);

            VirtualHandPosition.transform.position = Vector3.Lerp(RealHandPosition.transform.position, OffsetHand.transform.position, lerpValue);

            targetDistanceFromCenter = Vector3.Distance(currentTargetElbowSelected.transform.position, centerTargetElbow.transform.position);
            distanceTraveled = Vector3.Distance(RealElbowPosition.transform.position, centerTargetElbow.transform.position);

            lerpValue = Mathf.Clamp01(distanceTraveled / targetDistanceFromCenter);

            VirtualElbowPosition.transform.position = Vector3.Lerp(RealElbowPosition.transform.position, OffsetElbow.transform.position, lerpValue);
        }
        else
        {
            VirtualHandPosition.transform.position = RealHandPosition.transform.position;
            VirtualElbowPosition.transform.position = RealElbowPosition.transform.position;
        }
    }

    /// <summary>
    /// Sets the rotation offsets for the elbow and hand targets based on the selected target hands and experimental trial.
    /// </summary>
    /// <param name="selectedTargetHand">The selected target hand.</param>
    /// <param name="selectedTargetElbow">The selected target elbow.</param>
    /// <param name="experimentalTrial">The experimental trial.</param>
    public void setRotationOffsetsOnTargets(ETargetHand selectedTargetHand, ETargetElbow selectedTargetElbow, ExperimentalTrial experimentalTrial)
    {
        this.experimentalTrial = experimentalTrial;

        if (selectedTargetElbow == ETargetElbow.R)
            return;

        int dominantHand = DominantHandPicker.Instance.dominantHand == EDominantHand.Right ? 1 : -1;
        Transform selectedElbowTarget = TargetsElbow.transform.Find(selectedTargetElbow.ToString());
        Transform selectedHandTarget = TargetsHand.transform.Find(selectedTargetHand.ToString());


        // Elbow offset setting
        int movementOffset =
            experimentalTrial.movementOffset == EMovementOffset.Raccourcissement && selectedTargetElbow == ETargetElbow.PM_PP
            ||
            experimentalTrial.movementOffset == EMovementOffset.Allongement && selectedTargetElbow == ETargetElbow.MM_MP
            ? 1 : -1;

        Vector3 direction = selectedElbowTarget.transform.position - TargetShoulder.transform.position;
        float angleInDegrees = movementOffset * dominantHand * experimentalTrial.shoulderAngleOffset;
        Quaternion rotation = Quaternion.AngleAxis(angleInDegrees, Vector3.up); // we assume that the targets are in the XZ plane
        Vector3 newDirection = rotation * direction;
        OffsetElbow.transform.position = TargetShoulder.transform.position + newDirection;
        // Elbow offset setting

        // Hand offset setting
        movementOffset =
            experimentalTrial.movementOffset == EMovementOffset.Allongement && (selectedTargetHand == ETargetHand.MM || selectedTargetHand == ETargetHand.PM)
            ||
            experimentalTrial.movementOffset == EMovementOffset.Raccourcissement && (selectedTargetHand == ETargetHand.PP || selectedTargetHand == ETargetHand.MP)
            ? 1 : -1;

        direction = selectedHandTarget.transform.position - selectedElbowTarget.position;
        angleInDegrees = movementOffset * dominantHand * experimentalTrial.elbowAngleOffset;
        rotation = Quaternion.AngleAxis(angleInDegrees, Vector3.up); // we assume that the targets are in the XZ plane
        newDirection = rotation * direction;
        OffsetHand.transform.position = selectedElbowTarget.position + newDirection;
        // Hand offset setting
    }

    /// <summary>
    /// Sets the rotation offsets on the bones of the dominant hand to match the selected target hand and elbow, according to the experimental trial settings.
    /// </summary>
    /// <param name="selectedTargetHand">The selected target hand.</param>
    /// <param name="selectedTargetElbow">The selected target elbow.</param>
    /// <param name="experimentalTrial">The current experimental trial settings.</param>
    /// <remarks>
    /// If the movement offset is "Congruent", the offset bones will be parented to the real bones and their position will be set to (0,0,0), effectively disabling the offset.
    /// Otherwise, the dominant hand is determined and the offset bones are unparented to allow them to move independently.
    /// The elbow offset is calculated based on the selected target elbow and the shoulder angle offset of the experimental trial.
    /// The hand offset is calculated based on the selected target hand and the elbow angle offset of the experimental trial, and it is scaled by 1.2 to compensate for the shorter bone length.
    /// </remarks>
    public void setRotationOffsetsOnBones(ETargetHand selectedTargetHand, ETargetElbow selectedTargetElbow, ExperimentalTrial experimentalTrial)
    {
        this.experimentalTrial = experimentalTrial;

        if (experimentalTrial.movementOffset == EMovementOffset.Congruent)
        {
            // Disable the offset by parenting it to the real bones and setting its position to (0,0,0)
            OffsetElbow.transform.parent = RealElbowPosition.transform;
            OffsetHand.transform.parent = RealHandPosition.transform;
            OffsetElbow.transform.localPosition = new Vector3(0f, 0f, 0f);
            OffsetHand.transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        int dominantHand = DominantHandPicker.Instance.dominantHand == EDominantHand.Right ? 1 : -1;
        OffsetElbow.transform.parent = null;
        OffsetHand.transform.parent = null;

        // Elbow offset setting
        int movementOffset =
            experimentalTrial.movementOffset == EMovementOffset.Raccourcissement && selectedTargetElbow == ETargetElbow.PM_PP
            ||
            experimentalTrial.movementOffset == EMovementOffset.Allongement && selectedTargetElbow == ETargetElbow.MM_MP
            ? 1 : -1;

        Vector3 direction = DominantHandPicker.Instance.RealElbowPosition.transform.position - DominantHandPicker.Instance.RealShoulderPosition.transform.position;
        float angleInDegrees = movementOffset * dominantHand * experimentalTrial.shoulderAngleOffset;
        Quaternion rotation = Quaternion.AngleAxis(angleInDegrees, Vector3.Cross(direction, DominantHandPicker.Instance.RealShoulderPosition.transform.forward).normalized); // TODO: Make sure that Vector3.Cross gives the axis plan that we desire to set offsets
        Vector3 newDirection = rotation * direction;
        OffsetElbow.transform.position = DominantHandPicker.Instance.RealShoulderPosition.transform.position + newDirection;
        OffsetElbow.transform.parent = RealElbowPosition.transform;
        // Elbow offset setting

        // Hand offset setting
        movementOffset =
            experimentalTrial.movementOffset == EMovementOffset.Allongement && (selectedTargetHand == ETargetHand.MM || selectedTargetHand == ETargetHand.PM)
            ||
            experimentalTrial.movementOffset == EMovementOffset.Raccourcissement && (selectedTargetHand == ETargetHand.PP || selectedTargetHand == ETargetHand.MP)
            ? 1 : -1;

        direction = DominantHandPicker.Instance.RealHandPosition.transform.position - DominantHandPicker.Instance.RealElbowPosition.transform.position;
        angleInDegrees = movementOffset * dominantHand * experimentalTrial.elbowAngleOffset;
        rotation = Quaternion.AngleAxis(angleInDegrees, Vector3.Cross(direction, DominantHandPicker.Instance.RealHandPosition.transform.forward).normalized); // TODO: Make sure that Vector3.Cross gives the axis plan that we desire to set offsets
        newDirection = rotation * direction;
        OffsetHand.transform.position = DominantHandPicker.Instance.RealElbowPosition.transform.position + newDirection * 1.2f;
        OffsetHand.transform.parent = RealHandPosition.transform;
        // Hand offset setting
    }
}
