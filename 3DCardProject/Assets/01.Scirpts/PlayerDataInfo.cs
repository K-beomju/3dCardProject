using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerDataInfo : MonoBehaviour
{
    [SerializeField] private Text hpText;
    [SerializeField] private Text goldText;
    [SerializeField] private Coin coin;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        Global.Pool.CreatePool<Coin>(coin.gameObject, player.transform);

    }


    private void Start()
    {
        DataInfoScreen();
    }

    public void DataInfoScreen()
    {
        hpText.text = SaveManager.Instance.gameData.Hp.ToString();
        goldText.text = SaveManager.Instance.gameData.Money.ToString();
    }

    [ContextMenu("GetGold")]
    public void GetGoldDirect()
    {
        StartCoroutine(GetGoldDirectCo());
    }

    private IEnumerator GetGoldDirectCo()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(.15f);
            coin = Global.Pool.GetItem<Coin>();
            coin.transform.position = player.transform.position + new Vector3(0, 2.5f, 0);
            coin.transform.rotation = Quaternion.Euler(90, 0, 0);
            coin.transform.DOLocalRotate(new Vector3(90, 90, 0), 1);
            coin.transform.DOMoveY(0.5f, .35f);

        }
    }
}
