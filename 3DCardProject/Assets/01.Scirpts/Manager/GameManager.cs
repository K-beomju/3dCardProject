using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

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
    private GameObject resultPanel;

    public GameState State { get; set; }
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
        exitBtn.onClick.AddListener(()=> {
            SceneManager.LoadScene("Chapter_1");
        });

        OnWinGame += (() => {
            resultPanel.SetActive(true);
        });
    }
    public void CallOnWinGame()
    {
        print("GAMEWIN");
        ReflectBox.Instance.ReflectBoxActive(false);
        State = GameState.END;
        resultPanel.SetActive(true);
    }
    public void CallOnLoseGame()
    {
        print("GAMELOSE");
    }
}
