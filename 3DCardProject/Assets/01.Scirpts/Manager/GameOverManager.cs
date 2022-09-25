using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameOverManager : Singleton<GameOverManager>
{
    public GameObject GameOverUIPrefab;
    private GameObject gameOverUI;
    private CanvasGroup cg;
    private void Start()
    {
        gameOverUI = Instantiate(GameOverUIPrefab, transform);
        cg = gameOverUI.GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }
    [ContextMenu("GAMEOVER")]
    public void GameOver()
    {
        StartCoroutine(GameOverCol());
    }
    public IEnumerator GameOverCol()
    {
        switch (StageManager.Instance.SceneState)
        {
            case SceneState.Title:
                break;
            case SceneState.STAGE:
                break;
            case SceneState.BATTLE:
                yield return BattleCameraController.FocusOnPlayer();
                break;
            case SceneState.SHOP:
                break;
            default:
                break;
        }

        yield return cg.DOFade(1, 1f);
    }
}
