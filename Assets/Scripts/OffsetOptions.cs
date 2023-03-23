using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Old method to set offsets located on controller. Use RotationOffsetOptions.cs instead for offsets located bones.", false)]
public class OffsetOptions : MonoBehaviour
{
    public GameObject LeftVirtualHand;
    public GameObject RightVirtualHand;
    public GameObject LeftRealHand;
    public GameObject RightRealHand;

    private GameObject VirtualHand;
    private GameObject RealHand;

    public GameObject OffsetDefault;
    public GameObject OffsetFurtherHorizontal;
    public GameObject OffsetNearerHorizontal;
    public GameObject OffsetFurtherVertical;
    public GameObject OffsetNearerVertical;

    public GameObject centerTarget;

    public GameObject currentTargetHandSelected { get; set; }

    public EOffset offset = EOffset.Default;

    void Start()
    {
        if (DominantHandPicker.Instance is not null)
        {
            if (DominantHandPicker.Instance.dominantHand == EDominantHand.Left)
            {
                VirtualHand = LeftVirtualHand;
                RealHand = LeftRealHand;
                OffsetDefault.transform.parent.transform.rotation = Quaternion.Euler(180, OffsetDefault.transform.parent.transform.eulerAngles.y + 180, 0);
            }
            else
            {
                VirtualHand = RightVirtualHand;
                RealHand = RightRealHand;
            }
        }
        else
        {
            VirtualHand = RightVirtualHand;
            RealHand = RightRealHand;
        }
        OffsetDefault.transform.parent.parent = VirtualHand.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        switch (offset)
        {
            case EOffset.Default:
                VirtualHand.transform.position = OffsetDefault.transform.position;
                break;
            case EOffset.Further:
                if(currentTargetHandSelected != null)
                {
                    // With this code, the offset depends on the distance with the selected target, so the nearer it gets to the selected target, lerps tends to 1
                    /* 
                    float totalDistance = Vector3.Distance(centerTarget.transform.position, currentTargetSelected.transform.position);
                    float remainingDistance = Vector3.Distance(RealHand.transform.position, currentTargetSelected.transform.position);
                    float distanceTraveled = Mathf.Max(totalDistance - remainingDistance, 0);

                    float lerpValue = distanceTraveled / totalDistance;
                    */

                    // With this code, the offset depends only on the distance travelled from the center, so the more it gets further from the center, lerp tends to 1
                    float targetDistanceFromCenter = Vector3.Distance(currentTargetHandSelected.transform.position, centerTarget.transform.position);
                    float distanceTraveled = Vector3.Distance(RealHand.transform.position, centerTarget.transform.position);

                    float lerpValue = Mathf.Clamp01(distanceTraveled / targetDistanceFromCenter);
                    //

                    switch (currentTargetHandSelected.name)
                    {
                        case "Avant":
                            VirtualHand.transform.position = Vector3.Lerp(OffsetDefault.transform.position, OffsetFurtherVertical.transform.position, lerpValue);
                            break;
                        case "Extérieure":
                            VirtualHand.transform.position = Vector3.Lerp(OffsetDefault.transform.position, OffsetFurtherHorizontal.transform.position, lerpValue);
                            break;
                        case "Arrière":
                            VirtualHand.transform.position = Vector3.Lerp(OffsetDefault.transform.position, OffsetNearerVertical.transform.position, lerpValue);
                            break;
                        case "Intérieure":
                            VirtualHand.transform.position = Vector3.Lerp(OffsetDefault.transform.position, OffsetNearerHorizontal.transform.position, lerpValue);
                            break;
                        default:
                            VirtualHand.transform.position = OffsetFurtherHorizontal.transform.position;
                            break;
                    }
                    //Debug.Log("Lerp " + lerpValue);
                }
                else
                {
                    VirtualHand.transform.position = OffsetFurtherHorizontal.transform.position;
                }
                break;
            case EOffset.Nearer:
                if (currentTargetHandSelected != null) 
                {
                    // With this code, the offset depends on the distance with the selected target, so the nearer it gets to the selected target, lerps tends to 1
                    /* 
                    float totalDistance = Vector3.Distance(centerTarget.transform.position, currentTargetSelected.transform.position);
                    float remainingDistance = Vector3.Distance(RealHand.transform.position, currentTargetSelected.transform.position);
                    float distanceTraveled = Mathf.Max(totalDistance - remainingDistance, 0);

                    float lerpValue = distanceTraveled / totalDistance;
                    */

                    // With this code, the offset depends only on the distance travelled from the center, so the more it gets further from the center, lerp tends to 1
                    float targetDistanceFromCenter = Vector3.Distance(currentTargetHandSelected.transform.position, centerTarget.transform.position);
                    float distanceTraveled = Vector3.Distance(RealHand.transform.position, centerTarget.transform.position);

                    float lerpValue = Mathf.Clamp01(distanceTraveled / targetDistanceFromCenter);
                    //

                    switch (currentTargetHandSelected.name)
                    {
                        case "Avant":
                            VirtualHand.transform.position = Vector3.Lerp(OffsetDefault.transform.position, OffsetNearerVertical.transform.position, lerpValue);
                            break;
                        case "Extérieure":
                            VirtualHand.transform.position = Vector3.Lerp(OffsetDefault.transform.position, OffsetNearerHorizontal.transform.position, lerpValue);
                            break;
                        case "Arrière":
                            VirtualHand.transform.position = Vector3.Lerp(OffsetDefault.transform.position, OffsetFurtherVertical.transform.position, lerpValue);
                            break;
                        case "Intérieure":
                            VirtualHand.transform.position = Vector3.Lerp(OffsetDefault.transform.position, OffsetFurtherHorizontal.transform.position, lerpValue);
                            break;
                        default:
                            VirtualHand.transform.position = OffsetNearerHorizontal.transform.position;
                            break;
                    }
                }
                else
                {
                    VirtualHand.transform.position = OffsetNearerHorizontal.transform.position;
                }
                break;
        }
    }
}
