using Assets.Scripts.Enums;
using DitzelGames.FastIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCalibration_V2 : MonoBehaviour
{
    private DominantHandPicker dominantHand;

    // Start is called before the first frame update
    void Start()
    {
        if(DominantHandPicker.Instance is not null)
        {
            dominantHand = DominantHandPicker.Instance;

            switch (DominantHandPicker.Instance.dominantHand)
            {

                case EDominantHand.Left:
                    activateFastIK(dominantHand.leftElbow);
                    deactivateFastIK(dominantHand.rightElbow);

                    setFastIKOptions(dominantHand.leftHand, 10, 1);
                    setFastIKOptions(dominantHand.rightHand, 500, 2);

                    activateFastIK(dominantHand.leftHand);
                    activateFastIK(dominantHand.rightHand);
                    break;

                case EDominantHand.Right:
                    activateFastIK(dominantHand.rightElbow);
                    deactivateFastIK(dominantHand.leftElbow);

                    setFastIKOptions(dominantHand.rightHand, 10, 1);
                    setFastIKOptions(dominantHand.leftHand, 500, 2);

                    activateFastIK(dominantHand.leftHand);
                    activateFastIK(dominantHand.rightHand);
                    break;
            }
        }
    }

    void activateFastIK(GameObject bone)
    {
        bone.GetComponent<FastIKFabric>().enabled = true;
    }

    void deactivateFastIK(GameObject bone)
    {
        bone.GetComponent<FastIKFabric>().enabled = false;
    }

    void setFastIKOptions(GameObject bone, int iterations, int chainLength)
    {
        bone.GetComponent<FastIKFabric>().Iterations = iterations;
        bone.GetComponent<FastIKFabric>().ChainLength = chainLength;
    }

    void calibrateEntireArmDistance(float value, GameObject hand, GameObject elbow, bool isDominantHand)
    {
        if(isDominantHand)
            deactivateFastIK(elbow);
        deactivateFastIK(hand);

        hand.transform.localPosition = new Vector3(hand.transform.localPosition.x, hand.transform.localPosition.y + value, hand.transform.localPosition.z);
        elbow.transform.localPosition = new Vector3(elbow.transform.localPosition.x, elbow.transform.localPosition.y + value, elbow.transform.localPosition.z);

        if (isDominantHand)
            activateFastIK(elbow);
        activateFastIK(hand);
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
            calibrateEntireArmDistance(0.0005f, dominantHand.leftHand, dominantHand.leftElbow, DominantHandPicker.Instance.dominantHand == EDominantHand.Left);
            calibrateEntireArmDistance(0.0005f, dominantHand.rightHand, dominantHand.rightElbow, DominantHandPicker.Instance.dominantHand == EDominantHand.Right);
            scaleArmature(0.5f);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            calibrateEntireArmDistance(-0.0005f, dominantHand.leftHand, dominantHand.leftElbow, DominantHandPicker.Instance.dominantHand == EDominantHand.Left);
            calibrateEntireArmDistance(-0.0005f, dominantHand.rightHand, dominantHand.rightElbow, DominantHandPicker.Instance.dominantHand == EDominantHand.Right);
            scaleArmature(-0.5f);
        }
    }
}
