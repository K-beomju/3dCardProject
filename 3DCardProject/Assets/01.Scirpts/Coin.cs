using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Detactive());
    }

    private IEnumerator Detactive()
    {
        yield return new WaitForSeconds(.35f);
        StageManager.Instance.GetCoinEffect(this.transform);
        gameObject.SetActive(false);
    }    
   
}
