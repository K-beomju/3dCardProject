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
            NameText.text = itemData.itemName.ToString();
            ExplainText.text = itemData.description.ToString();
            ActiveUI(true);
        }
    }

    public void ActiveUI(bool isActive)
    {
        infoUIObj.SetActive(isActive);
    }
    private void Update()
    {
        if(infoUIObj.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition;
            infoUIObj.transform.position = mousePos;
        }

    }
}
