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

    [SerializeField]
    private UnityEngine.UI.Button resetButton;

    private void Start()
    {
        resetButton.onClick.AddListener(ResetIsFirstData);
        for (int i = 0; i < cardPosTrm.Length; i++)
        {
            if (cards.Length == i) break;
            if(2 == i ||( 1 == i && SaveManager.Instance.gameData.isFirst))
            {
                continue;
            }
            Card card = Global.Pool.GetItem<Card>();
            card.transform.localScale = new Vector3(.35f, .35f, .35f);
            card.Setup(cards[i].item, true, true);
            card.transform.position = cardPosTrm[i].position;
            card.transform.rotation = cardPosTrm[i].rotation;
            card.originPRS = new PRS(card.transform);
        }
    }

    [ContextMenu("ResetIsFirstData")]
    public void ResetIsFirstData()
    {
        SecurityPlayerPrefs.DeleteKey("IsFirst");
    }
}
