using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderCalibration : MonoBehaviour
{
    public GameObject ArmIK;
    float shoulderRotationY;

    // Start is called before the first frame update
    void Start()
    {
        setShouldersRotation();
    }

    void setShouldersRotation()
    {
        shoulderRotationY = transform.parent.eulerAngles.y;
        ArmIK.transform.eulerAngles = new Vector3(0.0f, transform.parent.eulerAngles.y, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            setShouldersRotation();
        }
        transform.rotation = Quaternion.Euler(0.0f, shoulderRotationY, 0.0f);
    }
}