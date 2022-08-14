using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Effect : MonoBehaviour
{
    public float lifeTime = 1f;

    protected WaitForSeconds lifeWait = null;

    public Action OnDisable;

    protected void Awake()
    {
        lifeWait = new WaitForSeconds(lifeTime);
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(LifeTime());
    }

    protected IEnumerator LifeTime()
    {
        yield return lifeWait;
        OnDisable?.Invoke();
        gameObject.SetActive(false);
    }
}
