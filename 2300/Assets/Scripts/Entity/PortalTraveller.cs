using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public virtual void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
