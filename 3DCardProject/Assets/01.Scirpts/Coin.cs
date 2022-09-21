using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool isEffect = false;
    public float detaCount;

    void Start()
    {
        StartCoroutine(Detactive());
    }

    private IEnumerator Detactive()
    {
        yield return new WaitForSeconds(detaCount);
        if(isEffect)
        BoardManager.Instance.GetCoinEffect(this.transform);
        gameObject.SetActive(false);
    }    
   
}
