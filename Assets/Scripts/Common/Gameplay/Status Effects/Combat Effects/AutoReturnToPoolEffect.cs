using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AutoReturnToPoolEffect : MonoBehaviour 
{
    [SerializeField] private float duration = 1;
    [SerializeField] private ParticleSystem particle;

    private float _elapsedTime = 0;

    public ObjectPool<ParticleSystem> Pool;

    private void OnEnable()
    {
        _elapsedTime = 0;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if(_elapsedTime > duration)
        {
            Pool.Release(particle);
        }
    }
}
