using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardInfoUI : Singleton<CardInfoUI>
{

    public GameObject infoUIObj;
    public TMP_Text NameText;
    public TMP_Text ExplainText;

    private Item itemData;
    public Item ItemData
    {
        get { return itemData; }
        set
        { 
            itemData = value;
            NameText.text = itemData.name.ToString();
            ExplainText.text = itemData.description.ToString();
            ActiveUI(true);
        }
    }

    public void ActiveUI(bool isActive)
    {
        infoUIObj.SetActive(isActive);
    }
}
