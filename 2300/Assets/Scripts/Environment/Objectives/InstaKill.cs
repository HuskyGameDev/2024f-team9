using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKill : Objective
{

    [Header("Instant Kill")]
    public LayerMask enemyLayer;
    public float explosionRadius = 8;
    public float explosionVfxEndAfter = 2;

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
        StartCoroutine(ExplodeFX());
        var results = Physics2D.OverlapCircleAll(go.transform.position, explosionRadius, enemyLayer);
        foreach (var result in results)
        {
            result.gameObject.SetActive(false);
        }
    }

    private IEnumerator ExplodeFX()
    {
        _ps?.Play();
        yield return new WaitForSeconds(explosionVfxEndAfter);
        _ps?.Stop();
    }
}
