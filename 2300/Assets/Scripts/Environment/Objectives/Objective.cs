using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    [Header("Objective")]
    [SerializeField]
    private Slider progressBar;
    protected float _progress
    {
        get { return progressBar.value; }
        set
        {
            var val = Mathf.Clamp01(value);
            progressBar.value = val;
        }
    }
    private bool _playerDetected = false;

    [Header("Fill")]
    public float maxFill = 1, minFill = 0;
    [Tooltip("How fast does the Progress Bar Fill each second?")]
    public float fillRate = 0.1f;

    [Header("Decay")]
    [Tooltip("How fast does the Progress Bar Decay?")]
    public float decayRate = 0.05f;
    [Tooltip("When does the progress bar start decaying?")]
    public float decayStartAfter = 3;
    private float _decayTime;

    [Header("Player Detection")]
    [Tooltip("How close does the player have to be to start the objective?")]
    public float detectionRange = 2;
    public LayerMask playerLayer;

    [HideInInspector]
    public UnityEvent<GameObject> progressFinished;
    private bool _recharge;

    protected void Awake()
    {
        if (progressBar == null)
        {
            var sl = gameObject.GetComponentInChildren<Slider>();
            if (!sl) // if no child has this component then look in your children for the component.
            {
                for (int i = 0; i < transform.childCount; i++) // loop over all children and check for slider component in their children
                {
                    var c = transform.GetChild(i);
                    sl = c.GetComponentInChildren<Slider>();
                    if (sl) { progressBar = sl; return; }
                }
            }
            else progressBar = sl;
        }
    }

    protected void Start()
    {
        if (progressBar)
        {
            progressBar.maxValue = maxFill;
            progressBar.minValue = minFill;

            _progress = minFill;
        }
        StartCoroutine(decay());
        StartCoroutine(fill());
    }

    // Update is called once per fixed framerate frame
    protected void FixedUpdate()
    {
        if (!_playerDetected && _decayTime >= decayStartAfter)
        {
            _decay = true;
            _fill = false;
        }

        if (Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer)) // if the player is found by the colliders
        {
            _fill = true;
            _decay = false;
            _playerDetected = true;
            _decayTime = 0;
        }
        else
        {
            _fill = false;
            _playerDetected = false;
            _decayTime += Time.fixedDeltaTime;
        }

        if (_recharge)
        {
            _fill = false;
            _decay = false;
        }
    }


    private bool _decay = false;
    private IEnumerator decay()
    {
        var wait = new WaitForSeconds(1);

        while (true)
        {

            yield return wait; // wait 1 second before decaying progress bar
            if (_decay || _recharge)
                _progress -= decayRate;

            if (_progress <= minFill)
                _recharge = false;
        }
    }

    private bool _fill = false;
    private IEnumerator fill()
    {
        var wait = new WaitForSeconds(1);

        while (true)
        {
            yield return wait; // wait 1 second before filling progress bar
            if (_fill)
                _progress += fillRate;

            if (_progress >= maxFill)
            {
                progressFinished.Invoke(gameObject);
                _recharge = true;
            }
        }
    }
}
