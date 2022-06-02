using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Action OnWinGame;
    public Action OnLoseGame;

    [SerializeField]
    private Button addBtn;
    [SerializeField]
    private Button exitBtn;
    [SerializeField]
    private GameObject resultPanel;
    public ItemSO item;

    protected override void Awake()
    {
        base.Awake();
        OnWinGame += CallOnWinGame;
        OnLoseGame += CallOnLoseGame;
    }
    private void Start()
    {
        resultPanel.SetActive(false);
        exitBtn.onClick.AddListener(()=> {
            SceneManager.LoadScene("StageScene");
        });
        addBtn.onClick.AddListener(() =>
        {
            FindObjectOfType<PlayerDeckManager>().AddCardToDeck(item.item.ShallowCopy());
            addBtn.gameObject.SetActive(false);
        });
        OnWinGame += (() => {
            resultPanel.SetActive(true);
            addBtn.gameObject.SetActive(true);
        });
    }
    public void CallOnWinGame()
    {
        print("GAMEWIN");
        resultPanel.SetActive(true);
    }
    public void CallOnLoseGame()
    {
        print("GAMELOSE");
    }
}
