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

    public GameState State { get; set; }

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
        State = GameState.RUNNING;
        resultPanel.SetActive(false);
        turnPanel.SetActive(true);
     
        exitBtn.onClick.AddListener(()=> {
            SceneManager.LoadScene("Chapter_1");
        });
        reStartBtn.onClick.AddListener(() =>{
            SceneManager.LoadScene("MinSangSang");
        });


            fadeGroup.DOFade(0, 2);
    }
    public void CallOnWinGame()
    {
        resultText.text = "½Â¸®";
        TurnManager.Instance.CanChangeTurn = false;
        resultPanel.SetActive(true);
        ReflectBox.Instance.ReflectBoxActive(false);
        State = GameState.END;
        turnPanel.SetActive(false);
    }
    public void CallOnLoseGame()
    {
        resultText.text = "ÆÐ¹è";
        TurnManager.Instance.CanChangeTurn = false;
        resultPanel.SetActive(true);
        ReflectBox.Instance.ReflectBoxActive(false);
        State = GameState.END;
        turnPanel.SetActive(false);

    }


    public void ChangeDirection()
    {
        if (NewFieldManager.Instance.IsClockDir)
           timeDirImage.transform.DORotate(new Vector3(90, 40, 0), 1);
        else
            timeDirImage.transform.DORotate(new Vector3(-90, -40, 0), 1);


    }
}
