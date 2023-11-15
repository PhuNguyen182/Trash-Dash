using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleEffectPool : MonoBehaviour
{
    private int _capacity = 20;
    private int _maxSizePool = 10000;
    private bool _isCollectionCheck = true;
    private ParticleSystem _prefab;

    private ObjectPool<ParticleSystem> _effectPool;

    public ObjectPool<ParticleSystem> EffectPool
    {
        get
        {
            if (_effectPool == null)
            {
                _effectPool = new ObjectPool<ParticleSystem>(CreateEffect, OnTakeFromPool
                                                             , OnReturnToPool, OnDestroyPoolObject
                                                             , _isCollectionCheck, _capacity, _maxSizePool);
            }

            return _effectPool;
        }
    }

    public ParticleSystem Spawn(ParticleSystem particle, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        _prefab = particle;
        ParticleSystem effect = EffectPool.Get();
        //effect.transform.SetTRP(position, rotation, parent);

        return effect;
    }

    public void Despawn(ParticleSystem particle)
    {
        EffectPool.Release(particle);
    }

    private ParticleSystem CreateEffect()
    {
        ParticleSystem particle = Instantiate(_prefab);

        if (particle.TryGetComponent<AutoReturnToPoolEffect>(out var returnToPool))
            returnToPool.Pool = EffectPool;

        return particle;
    }

    private void OnTakeFromPool(ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);
    }

    private void OnReturnToPool(ParticleSystem particle)
    {
        particle.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(ParticleSystem particle)
    {
        Destroy(particle.gameObject);
    }
}
