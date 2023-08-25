using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform objectToFollow;

    public Vector3 offset;

    private void Start()
    {
        this.gameObject.transform.SetParent(objectToFollow);
    }
    void Update()
    {

    }
}
