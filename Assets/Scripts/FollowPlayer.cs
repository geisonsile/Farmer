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
        transform.position = objectToFollow.position + offset;
    }

}
