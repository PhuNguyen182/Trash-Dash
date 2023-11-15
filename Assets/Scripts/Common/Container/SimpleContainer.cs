using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleContainer : MonoBehaviour { }

public class SimpleContainer<T> : SimpleContainer
{
    private static SimpleContainer _instance = null;

    public static SimpleContainer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject($"{typeof(T)}")
                                .AddComponent<SimpleContainer>();
            }

            return _instance;
        }
    }

    public static Transform InstanceTransform => Instance.transform;

    public static void TakeChild(Transform transform)
    {
        transform.SetParent(InstanceTransform);
    }
}

public class EffectContainer : SimpleContainer<EffectContainer> { }

public class TrackContainer : SimpleContainer<TrackContainer> { }

public class CurrencyContainer : SimpleContainer<CurrencyContainer> { }

public class PowerupContainer : SimpleContainer<PowerupContainer> { }
