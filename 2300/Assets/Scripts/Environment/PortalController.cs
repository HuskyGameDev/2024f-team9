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
    List<PortalTraveller> _travelers;

    // Start is called before the first frame update
    void Start()
    {
        _BC = GetComponent<BoxCollider2D>();
        _BC.size = new Vector2(_BC.size.x, tileSizeY);
        screen.transform.localScale = new Vector3(screen.transform.localScale.x, tileSizeY);
        _travelers = new List<PortalTraveller>();
    }

}


