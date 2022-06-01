using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAddBtn : MonoBehaviour
{
    public ItemSO item;
    private Button addBtn;
    private void Start()
    {
        addBtn = GetComponent<Button>();
        addBtn.onClick.AddListener(() => 
        {
            FindObjectOfType<PlayerDeckManager>().AddCardToDeck(item.item.ShallowCopy());
            gameObject.SetActive(false);
        });
        GameManager.Instance.OnWinGame += (()=> { gameObject.SetActive(true); });
    }
}
