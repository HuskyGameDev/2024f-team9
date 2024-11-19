using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passiveRegen : MonoBehaviour
{
    public HealthManager healthScript; // Reference to the shooting script
    private float healthAmount;
    private float regen;

    void Start()
    {
        if (healthScript != null)
        {
            healthAmount = healthScript.healthAmount; // Store the original health
        }
    }

    // Call this method when the modifier is selected
    public void ApplyRegenModifier()
    {
        if (healthScript != null)
        {
            
            if (regen < 2){
                regen = regen +0.1f;
            }
            
            healthScript.healthAmount = healthAmount;
        }
    }


    
}
