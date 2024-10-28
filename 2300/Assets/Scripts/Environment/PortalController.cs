using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public bool LeftPortal = false;
    public PortalController linkedPortal;
    public SpriteRenderer screen;
    [HideInInspector]
    public int tileSizeX, tileSizeY;

    BoxCollider2D _BC;
    List<PortalTraveler> _travelers;

    // Start is called before the first frame update
    void Start()
    {
        _BC = GetComponent<BoxCollider2D>();
        _BC.size = new Vector2(_BC.size.x, tileSizeY);
        screen.transform.localScale = new Vector3(screen.transform.localScale.x, tileSizeY);
        _travelers = new List<PortalTraveler>();
    }

    private void LateUpdate()
    {
        handleTraveler();
    }

    void handleTraveler()
    {
        for(int i =0; i< _travelers.Count; i++)
        {
            PortalTraveler traveler = _travelers[i];
            var m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * traveler.transform.localToWorldMatrix;

            Vector3 offset = traveler.transform.position - transform.position;
            int portalSide = System.Math.Sign(Vector3.Dot(offset, transform.right));
            int portalSideOld = System.Math.Sign(Vector3.Dot(traveler.previousOffsetFromPortal, transform.right));

            // crossed the portal?
            if(portalSide != portalSideOld)
            {
                var posOld = traveler.transform.position;
                var rotOld = traveler.transform.rotation;

                traveler.Teleport(transform,linkedPortal.transform,m.GetColumn(3), m.rotation);
                linkedPortal.OnTravelerEnterPortal(traveler);
                _travelers.RemoveAt(i);
                i--;
            }
            else
            {
                traveler.previousOffsetFromPortal = offset;
            }

        }
    }

    void OnTravelerEnterPortal(PortalTraveler pt)
    {
        if (!_travelers.Contains(pt))
        {
            pt.previousOffsetFromPortal = pt.transform.position - transform.position;
            _travelers.Add(pt);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PortalTraveler>(out var T))
        {
            OnTravelerEnterPortal(T);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PortalTraveler>(out var T) && _travelers.Contains(T))
        {
            _travelers.Remove(T);
        }
    }

}


