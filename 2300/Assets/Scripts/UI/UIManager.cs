using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject dealthPanel;

    public void ToggleDeathPanel()
    {
        dealthPanel.SetActive(!dealthPanel.activeSelf);
    }
}
