using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public Rigidbody thisRigidbody;
    
    void Update()
    {
        /*transform.position = objectToFollow.position + offset;
       
        transform.position = transform.position + new Vector3(-0.2f, 0.75f, 0.2f) + transform.forward;
        transform.rotation = transform.rotation * Quaternion.Euler(-5f, -180f, 0);*/
    }

}
