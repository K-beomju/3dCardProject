using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;



public class TotemMove : MonoBehaviour
{
    [SerializeField] private BoardManager board;

    [SerializeField] private float speed;
    [SerializeField] private int routePosition;
    [SerializeField] private int steps;
    private bool isMove = false;

    #region Dice
    [SerializeField] private GameObject dice;
    [SerializeField] Camera cam;

    private Text diceText;
    private bool isRoll = false;

    private WaitForSeconds rollDelay = new WaitForSeconds(0.1f);
    private IEnumerator rollCo;
    #endregion

    [SerializeField] private CanvasGroup fadeGroup;

    public enum FadeType
    {
        FadeIn,
        FadeOut
    }
    private FadeType fadeType;


    private void Awake()
    {
        diceText = dice.GetComponent<Text>();
        rollCo = RollingDice();
        dice.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isRoll)
            {
                dice.SetActive(true);
                StartCoroutine(rollCo);
                isRoll = true;
            }
            else
            {
                StopCoroutine(rollCo);
                isRoll = false;


                if (!isMove)
                {
                    if (routePosition + steps < board.childNodeList.Count)
                    {
                        StartCoroutine(MoveMentCo());
                    }
                }
            }
        }

        if (isMove)
            dice.transform.position = cam.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));


    }

    private IEnumerator MoveMentCo()
    {
        yield return new WaitForSeconds(0.5f);

        if (isMove) yield break;
        isMove = true;
        yield return new WaitForSeconds(1f);

        while (steps > 0)
        {
            Vector3 nextPos = board.childNodeList[routePosition + 1].transform.position;
            while (MoveNextNode(nextPos))
                yield return null;

            // 보드 클리어 검사 
            board.ClearBoard(routePosition);
            yield return new WaitForSeconds(0.1f);
            steps--;
            diceText.text = steps.ToString();
            routePosition++;
            board.ChangeCam();

        }

        yield return new WaitForSeconds(0.5f);

        if(board.boardList[routePosition].type == StageType.Battle) // 만약 도착한 노드의 타입이 배틀이라면
        FadeInOut(FadeType.FadeOut, BattleScene);

        isMove = false;
        dice.SetActive(false);
    }

    private IEnumerator RollingDice()
    {
        while (true)
        {
            steps = UnityEngine.Random.Range(1, 7);
            diceText.text = steps.ToString();
            yield return rollDelay;
        }
    }


    private bool MoveNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }


    private void FadeInOut(FadeType fade, Action action = null)
    {
        switch (fade)
        {
            case FadeType.FadeIn:
                {
                    fadeGroup.DOFade(1, 0);
                    fadeGroup.DOFade(0, 1).OnComplete(() =>
                    {
                        action();
                    });
                }
                break;
            case FadeType.FadeOut:
                {
                    fadeGroup.DOFade(0, 1);
                    fadeGroup.DOFade(1, 1).OnComplete(() =>
                    {
                        action();
                    });
                }
                break;
        }
    }

    private void BattleScene()
    {
        SceneManager.LoadScene("MinSangSang");
    }

}
