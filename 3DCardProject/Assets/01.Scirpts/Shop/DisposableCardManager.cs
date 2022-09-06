using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableCardManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("DisposableCardManager");
        StartCoroutine(SpawnDisposableCard());
    }
    private IEnumerator SpawnDisposableCard()
    {
        yield return new WaitForSeconds(1f);
        if (SaveManager.Instance.gameData.DisposableItem == null) yield break;
        Card disposableCard = Global.Pool.GetItem<Card>();
        disposableCard.Setup(SaveManager.Instance.gameData.DisposableItem,true,true);
        disposableCard.isDisposable = true;
        disposableCard.OnSpawnAct += () => {
            SaveManager.Instance.gameData.DisposableItem = null;
        };
        foreach (CardActionCondition item in disposableCard.item.OnSpawn)
        {
            if (item.action is CardActionMove)
            {
                item.action = null;
                break;
            }
        }
        disposableCard.transform.position = transform.position;
    }
}
