using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public enum GameState
{
    PREPARE,
    RUNNING,
    END
}

public class GameManager : Singleton<GameManager>
{
    public Action OnWinGame;
    public Action OnLoseGame;

    [SerializeField]
    private Button exitBtn;
    [SerializeField]
    private Button reStartBtn;
    [SerializeField]
    private GameObject resultPanel;
    [SerializeField] private GameObject turnPanel;
    public TMP_Text resultText;

    public GameState State;

    public CanvasGroup fadeGroup;

    [SerializeField] private RectTransform timeDirImage;

    protected override void Awake()
    {
        base.Awake();
        State = GameState.PREPARE;
        OnWinGame += CallOnWinGame;
        OnLoseGame += CallOnLoseGame;
    }
    private void Start()
    {
        EnemyAI.Instance.enemyType = StageManager.Instance.enemyType;
        State = GameState.RUNNING;
        resultPanel.SetActive(false);
        turnPanel.SetActive(true);

        exitBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Chapter_1");
        });
        reStartBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MinSangSang");
        });


        fadeGroup.DOFade(0, 2);
    }
    public void CallOnWinGame()
    {
        StartCoroutine(CallOnWinGameCo());
    }

    public IEnumerator CallOnWinGameCo()
    {
        yield return new WaitForSeconds(1f);
        EnemyManager.Instance.DeadParticle();
        EnemyManager.Instance.EnemyDie();
        yield return new WaitForSeconds(3f);
        resultText.text = "½Â¸®";
        TurnManager.Instance.CanChangeTurn = false;
        resultPanel.SetActive(true);
        ReflectBox.Instance.ReflectBoxActive(false);
        turnPanel.SetActive(false);
    }

    public void CallOnLoseGame()
    {
        StartCoroutine(CallOnLoseGameCo());
    }

    public IEnumerator CallOnLoseGameCo()
    {
        yield return new WaitForSeconds(1f);

        resultText.text = "ÆÐ¹è";
        TurnManager.Instance.CanChangeTurn = false;
        resultPanel.SetActive(true);
        ReflectBox.Instance.ReflectBoxActive(false);
        turnPanel.SetActive(false);
    }


    public void ChangeDirection()
    {
        Sequence seq = DOTween.Sequence();


        int dir = 1;
        if (!NewFieldManager.Instance.IsClockDir)
            dir = -1;

        Vector3 scale = timeDirImage.transform.localScale;

        seq.Append(timeDirImage.transform.DORotate(new Vector3(0, 1000 * dir, 0), 1).SetRelative(true));
        seq.Insert(.3f, timeDirImage.transform.DOScale(new Vector3(scale.x * -1, scale.y, scale.z), 0));
    }
}
