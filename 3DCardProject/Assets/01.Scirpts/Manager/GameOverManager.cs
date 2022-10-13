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
        cg.gameObject.SetActive(false);
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
                yield return BoardManager.Instance.ZoomInTotem();
                break;
            case SceneState.BATTLE:
                yield return BattleCameraController.ZoomInPlayer();
                break;
            case SceneState.SHOP:
                break;
            default:
                break;
        }
        cg.gameObject.SetActive(true);

        yield return cg.DOFade(1, 1f);
    }
}
