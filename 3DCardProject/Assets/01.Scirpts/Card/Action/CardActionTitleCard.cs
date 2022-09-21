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
                // 새 게임


                SaveManager.Instance.gameData.isFirst = false;
                SaveManager.Instance.SaveGameData();
                if (SaveManager.Instance.gameData.IsTutorialDone)
                {
                    TitleManager.Instance.CameraMoveAction(() => Global.LoadScene.LoadScene("Stage"));
                }
                else
                {
                    TitleManager.Instance.CameraMoveAction(() => Global.LoadScene.LoadScene("Tutorials"));
                }
                break;
            case 1:
                // 이어하기
                TitleManager.Instance.CameraMoveAction(() => Global.LoadScene.LoadScene("Stage"));
                break;
            case 2:
                // 메뉴
                break;
            case 3:
                // 나가기
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
