using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthbar;
    public float healthAmount = 100f;

    public Controller playerController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage){
        healthAmount -= damage;
        healthbar.fillAmount = healthAmount / 100f;

        if (healthAmount <= 0f) {
            healthAmount = 0f;
            playerController.PlayerDied();
        }
    }

    public void Heal(float healingAmount){
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthbar.fillAmount = healthAmount/100f;
    }
}
