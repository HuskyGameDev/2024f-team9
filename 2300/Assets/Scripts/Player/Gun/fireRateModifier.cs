using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateModifier : MonoBehaviour
{
    public shooting shootingScript; // Reference to the shooting script
    public HealthManager healthManager;
    private float originalFireRate;
    private float modifiedFireRate;

    public float lifestealAmount = 0f;

    void Start()
    {
        if (shootingScript != null)
        {
            originalFireRate = shootingScript.timeBetweenFiring; 
        }
    }

    // on button press
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

//on button press
    public void ApplyLifesteal()
    {
        if (healthManager != null)
        {
            if (lifestealAmount < 2){
                lifestealAmount = lifestealAmount +1f;
            }
            
        
        }
    }
}
