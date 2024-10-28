using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveler : MonoBehaviour
{
    [HideInInspector]
    public Vector3 previousOffsetFromPortal;

    public virtual void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
