using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    static Global s_instance;
    public static Global Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }

    static GameObject managementObj;

    #region Managers

    UIManager _ui = new UIManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    LoadingSceneManager _loadScene = new LoadingSceneManager();

    public static UIManager UI { get { return Instance._ui; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static LoadingSceneManager LoadScene { get { return Instance._loadScene; } }

    #endregion

    void Start()
    {
         Init();
    }

    void Update()
    {
        // 각 매니저들마다 업데이트

    }

    static void Init()
    {
        managementObj = GameObject.Find("@Manager");

        if (s_instance == null)
        {
            if (managementObj == null)
            {
                managementObj = new GameObject("@Manager");
                managementObj.AddComponent<Global>();
            }

            s_instance = managementObj.GetComponent<Global>();
            DontDestroyOnLoad(managementObj);

            //초기화들
            s_instance._sound.Init();
            Transform poolObjectBox = managementObj.transform.Find("@Pool");

            if (poolObjectBox == null)
            {
                poolObjectBox = new GameObject("@Pool").transform;
                poolObjectBox.SetParent(managementObj.transform);
            }

            s_instance._pool.Init(poolObjectBox);
            s_instance._loadScene.Init();
        }
    }

    public static void Clear()
    {
        // ...
        Sound.Clear();
        Pool.Clear();
    }
}