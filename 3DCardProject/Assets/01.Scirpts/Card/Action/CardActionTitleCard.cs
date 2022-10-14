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
                    SaveManager.Instance.gameData.IsTutorialDone = true;
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
                SaveManager.Instance.NewGame();
                TitleManager.Instance.CameraMoveAction(() => {
                    SaveManager.Instance.gameData.IsTutorialDone = false;
                    Global.LoadScene.LoadScene("Tutorials");
                    SoundManager.Instance.bgmVolume = 0.05f;
                    SoundManager.Instance.PlayBGMSound("Stage");
                });
                break;
            case 2:
                // 크레딧
                SaveManager.Instance.NewGame();
                TitleManager.Instance.CameraMoveAction(()=>Global.LoadScene.LoadScene("Credit"));
                break;
            case 3:
                // 나가기
#if UNITY_EDITOR
                SaveManager.Instance.NewGame();
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                SaveManager.Instance.NewGame();
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
