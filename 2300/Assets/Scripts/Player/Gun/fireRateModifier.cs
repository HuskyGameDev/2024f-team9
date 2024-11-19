using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireRateModifier : MonoBehaviour
{
    public shooting shootingScript; // Reference to the shooting script
    public HealthManager healthManager;
    private float originalFireRate;
    public float fireRate = -0.01f;
    public float minFireRate = 0.1f;
    private float modifiedFireRate;

    public float lifestealAmount = 0f;
    public float lifestealRate = .01f;
    public float newLifestealAmount;

    public GameObject txt_fireRate;
    public GameObject txt_lifesteal;

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

            if (originalFireRate > minFireRate)
            {
                modifiedFireRate = originalFireRate + fireRate;
            }
            originalFireRate = modifiedFireRate;
            shootingScript.timeBetweenFiring = modifiedFireRate;
            txt_fireRate?.GetComponent<TextMeshProUGUI>().SetText($"({modifiedFireRate})");
        }
    }

    //on button press
    public void ApplyLifesteal()
    {
        if (healthManager != null)
        {
            if (lifestealAmount < 2)
            {
                newLifestealAmount = lifestealAmount + lifestealRate;
            }
            lifestealAmount = newLifestealAmount;
            txt_lifesteal?.GetComponent<TextMeshProUGUI>().SetText($"({lifestealAmount})");
        }
    }
}
