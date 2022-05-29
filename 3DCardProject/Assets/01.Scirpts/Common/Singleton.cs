using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : Singleton where T : Singleton<T>
{
    private static T _instance;
    public static T Instance { get { return _instance; } }


    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }

}

public abstract class Singleton : MonoBehaviour
{


}


