using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : Singleton where T : Singleton<T>
{
    private static T _instance;
    public static T Instance { get { return _instance; } }

    public bool isDontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
            if (isDontDestroyOnLoad)
                DontDestroyOnLoad((T)this);
        }
    }

}

public abstract class Singleton : MonoBehaviour
{


}


