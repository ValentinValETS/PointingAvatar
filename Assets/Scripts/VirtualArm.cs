using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualArm : MonoBehaviour
{
    public Transform Joint1;
    public Transform Joint2;
    private LineRenderer virtualArm;

    // Start is called before the first frame update
    void Start()
    {
        virtualArm = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        virtualArm.SetPosition(0, Joint1.position);
        virtualArm.SetPosition(1, Joint2.position);
    }
}
