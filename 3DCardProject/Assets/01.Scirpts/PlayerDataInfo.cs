using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataInfo : MonoBehaviour
{
    [SerializeField] private Text hpText;
    [SerializeField] private Text goldText;

    private void Start()
    {
        DataInfoScreen();
    }

    public void DataInfoScreen()
    {
        hpText.text = SaveManager.Instance.gameData.Hp.ToString();
        goldText.text = SaveManager.Instance.gameData.Money.ToString();
    }
}
