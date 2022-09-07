using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] cardPosTrm;
    // 시작, 나가기, 메뉴
    [SerializeField]
    private ItemSO[] cards;


    private void Start()
    {
        for (int i = 0; i < cardPosTrm.Length; i++)
        {
            if (cards.Length == i) break;

            Card card = Global.Pool.GetItem<Card>();
            card.transform.localScale = new Vector3(.35f, .35f, .35f);
            card.Setup(cards[i].item, true, true);
            card.transform.position = cardPosTrm[i].position;
            card.transform.rotation = cardPosTrm[i].rotation;
            card.originPRS = new PRS(card.transform);
        }
    }
}
