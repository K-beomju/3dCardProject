using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActionTitleCard : CardAction
{
    public override void TakeAction(Card card)
    {
        switch (card.item.uid)
        {
            case 0:
                // �� ����
                SecurityPlayerPrefs.SetBool("IsFirst",false);
                SaveManager.Instance.SaveGameData();
                Global.LoadScene.LoadScene("Stage");
                break;
            case 1:
                // �̾��ϱ�
                SaveManager.Instance.LoadGameData();
                Global.LoadScene.LoadScene("Stage");
                break;
            case 2:
                // �޴�
                break;
            case 3:
                // ������
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
