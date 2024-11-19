using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvoPointManager : MonoBehaviour
{
    public Image evoPointBar; // Reference to the EvoPoint progress bar
    public GameObject powerUpsMenu; // Reference to the Power-Ups menu panel
    public float evoPoints = 0f; // Current EvoPoints
    public float maxEvoPoints = 100f; // Maximum EvoPoints

    void Start()
    {
        if (powerUpsMenu != null)
        {
            powerUpsMenu.SetActive(false); // Ensure the Power-Ups menu is hidden initially
        }
    }

    void Update()
    {
        evoPointBar.fillAmount = evoPoints / maxEvoPoints;

        // Check if the EvoPoint bar is full
        if (evoPoints >= maxEvoPoints)
        {
            OnEvoPointBarFull();
        }
    }

    public void AddEvoPoints(float points)
    {
        evoPoints += points;
        evoPoints = Mathf.Clamp(evoPoints, 0, maxEvoPoints);
        evoPointBar.fillAmount = evoPoints / maxEvoPoints;
    }

    public void ResetEvoPoints()
    {
        evoPoints = 0f;
        evoPointBar.fillAmount = evoPoints / maxEvoPoints;
    }

    void OnEvoPointBarFull()
    {
        // Pause the game and open the Power-Ups menu
        Time.timeScale = 0f; // Pause the game
        if (powerUpsMenu != null)
        {
            powerUpsMenu.SetActive(true); // Show the Power-Ups menu
        }
        Debug.Log("EvoPoints are full! Power-Ups menu opened.");
    }

    public void ClosePowerUpsMenu()
    {
        // Resume the game and close the Power-Ups menu
        Time.timeScale = 1f; // Resume the game
        if (powerUpsMenu != null)
        {
            powerUpsMenu.SetActive(false); // Hide the Power-Ups menu
        }
        ResetEvoPoints(); // Reset the EvoPoint bar
    }
}
