using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField] private bool hasContainer;
    [SerializeField] private float duration;

    private float timer = 0;

    private void Awake()
    {
        this.UpdateAsObservable().Subscribe(value => UpdateTimer()).AddTo(this);
    }

    public void SetDuration(float duration) => this.duration = duration;

    private void UpdateTimer()
    {
        timer += Time.deltaTime;
        if(timer > duration)
        {
            if (hasContainer)
                transform.SetParent(EffectContainer.InstanceTransform);

            SimplePool.Despawn(this.gameObject);
        }
    }

    private void OnDisable()
    {
        timer = 0;
    }
}
