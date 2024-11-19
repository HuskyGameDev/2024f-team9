using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateModifier : MonoBehaviour
{
    public shooting shootingScript; // Reference to the shooting script
    private float originalFireRate;
    private float modifiedFireRate;

    void Start()
    {
        if (shootingScript != null)
        {
            originalFireRate = shootingScript.timeBetweenFiring; // Store the original fire rate
        }
    }

    // Call this method when the modifier is selected
    public void ApplyFireRateModifier()
    {
        if (shootingScript != null)
        {
            
            if (originalFireRate > 0.01){
                modifiedFireRate = originalFireRate -0.01f;
            }
            originalFireRate = modifiedFireRate;
            shootingScript.timeBetweenFiring = modifiedFireRate;
        }
    }


    
}
