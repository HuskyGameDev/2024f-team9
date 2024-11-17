using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    [SerializeField]
    private Slider progressBar;
    [Range(0f, 1f), Tooltip("How fast does the Progress Bar Decay")]
    public float decayRate = 0.05f;
    [Tooltip("When does the progress bar start decaying?")]
    public float decayStartAfter = 2;

    public void Start()
    {
        if (progressBar == null)
        {
            var sl = gameObject.GetComponentInChildren<Slider>();
            if (!sl)
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    var c = transform.GetChild(i);
                    sl = c.GetComponentInChildren<Slider>();
                    if (sl) { progressBar = sl; return; }
                }
            }
            else progressBar = sl;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
