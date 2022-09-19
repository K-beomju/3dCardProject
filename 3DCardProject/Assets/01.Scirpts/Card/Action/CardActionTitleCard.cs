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
                SaveManager.Instance.gameData.isFirst = false;
                SaveManager.Instance.SaveGameData();
                if (SaveManager.Instance.gameData.IsTutorialDone)
                {
                    Global.LoadScene.LoadScene("Stage");
                }
                else
                {
                    Global.LoadScene.LoadScene("Tutorials");
                }
                break;
            case 1:
                // �̾��ϱ�
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
