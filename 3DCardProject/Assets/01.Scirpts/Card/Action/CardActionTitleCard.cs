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

                SaveManager.Instance.NewGame();

                TitleManager.Instance.CameraMoveAction(() => {
                    StageManager.Instance.SceneState = SceneState.STAGE;
                    Global.LoadScene.LoadScene("Stage");
                    SoundManager.Instance.bgmVolume = 0.05f;
                    SoundManager.Instance.PlayBGMSound("Stage");
                });

              /*  if (SaveManager.Instance.gameData.IsTutorialDone)
                {
                    TitleManager.Instance.CameraMoveAction(() => { 
                        StageManager.Instance.SceneState = SceneState.STAGE; 
                        Global.LoadScene.LoadScene("Stage");
                        SoundManager.Instance.bgmVolume = 0.05f;
                        SoundManager.Instance.PlayBGMSound("Stage");
                    });
                }
                else
                {
                    TitleManager.Instance.CameraMoveAction(() => {
                        Global.LoadScene.LoadScene("Tutorials");
                        SoundManager.Instance.bgmVolume = 0.05f;
                        SoundManager.Instance.PlayBGMSound("Stage");
                    });
                }*/
                break;
            case 1:
                // 전 이어하기
                // 튜토리얼
                    /*TitleManager.Instance.CameraMoveAction(() => {
                        StageManager.Instance.SceneState = SceneState.STAGE; 
                        Global.LoadScene.LoadScene("Stage");
                        SoundManager.Instance.bgmVolume = 0.05f;
                        SoundManager.Instance.PlayBGMSound("Stage");
                    });*/
                TitleManager.Instance.CameraMoveAction(() => {
                    Global.LoadScene.LoadScene("Tutorials");
                    SoundManager.Instance.bgmVolume = 0.05f;
                    SoundManager.Instance.PlayBGMSound("Stage");
                });
                break;
            case 2:
                // 크레딧
                TitleManager.Instance.CameraMoveAction(()=>Global.LoadScene.LoadScene("Credit"));
                break;
            case 3:
                // 나가기
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
