using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script builds off of the Objective class because we use the progress bar but to leave room for future objectives i didn't want to hardcode the functionality of the instakill objective into the base class.
/// </summary>
public class InstaKill : Objective
{

    [Header("Instant Kill")]
    public LayerMask enemyLayer;
    public float explosionRadius = 8;

    private ParticleSystem _ps;

    protected new void Awake()
    {
        base.Awake();
        _ps = GetComponentInChildren<ParticleSystem>();
        _ps?.Stop();
    }

    private void OnEnable()
    {
        progressFinished.AddListener(Explode);
    }

    private void OnDisable()
    {
        progressFinished.RemoveListener(Explode);
    }

    private void Explode(GameObject go)
    {
        _ps?.Play();
        var results = Physics2D.OverlapCircleAll(go.transform.position, explosionRadius, enemyLayer);
        foreach (var result in results)
        {
            result.gameObject.SetActive(false);
        }
    }
}
