using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;



public class TotemMove : MonoBehaviour
{
    [SerializeField] private BoardManager board;
    [SerializeField] private float speed;
    [SerializeField] private int steps;
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private GameObject diceObj;
    [SerializeField] private ParticleSystem diceParticle;
    [SerializeField] private ParticleSystem battleModelParticle;
    [SerializeField] private GameObject itemMark;
    [SerializeField] private Camera cam;

    public enum FadeType
    {
        FadeIn,
        FadeOut
    }
    public int routePosition;
    public int stageValue;

    private bool isMove = false;
    private bool isLock = false;
    private bool isStart = false;
    private Animator anim;
    private FadeType fadeType;

    #region Dice
    public GameObject battleFieldModel;
    public Text diceText;
    public float rotSpeed;
    public Ease ease;
    #endregion


    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    private void Start()
    {
        stageValue = PlayerPrefs.GetInt("StageValue");
        routePosition = stageValue;
        transform.position = board.childNodeList[stageValue].transform.position;
        diceObj.transform.DOMoveY(-0.1f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
        battleFieldModel.SetActive(false);
        itemMark.SetActive(false);
    }

    private float TimeLeft = 1.0f;
    private float nextTime = 0.0f;
    private bool bChRot = false;
    private void Update()
    {
        diceObj.transform.Rotate(new Vector3(30, 0, 30) * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) && !isLock)
        {
            if (!isStart)
            {
                diceObj.SetActive(true);
                diceObj.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), .5f,2);
                isStart = true;
            }
            else
            {

                anim.SetTrigger("Attack");
                isLock = true;
            }
        }

        if (isMove)
            diceText.transform.position = cam.WorldToScreenPoint(transform.position + new Vector3(0, 1.2f, 0));

    }

    private IEnumerator MoveMentCo()
    {
        yield return new WaitForSeconds(1f);
        if (isMove) yield break;
        isMove = true;
        yield return new WaitForSeconds(1f);

        while (steps > 0)
        {
            Vector3 nextPos = board.childNodeList[routePosition + 1].transform.position;
            Vector3 lookAtPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
            transform.LookAt(lookAtPos);

            while (MoveNextNode(nextPos))
                yield return null;

            // 보드 클리어 검사 
            board.ClearBoard(routePosition);
            if (board.childNodeList[routePosition].GetSiblingIndex() % 2 == 1)
            {
                steps--;
                diceText.text = steps.ToString();
            }

            routePosition++;

        }
        anim.SetBool("isMove", false);
        yield return new WaitForSeconds(0.5f);
        diceText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);


        if (board.boardList[routePosition - 1].type == StageType.Battle) // 만약 도착한 노드의 타입이 배틀이라면
        {

            Vector3 lookAtPos = new Vector3(transform.position.x, battleFieldModel.transform.position.y, transform.position.z);

            battleFieldModel.SetActive(true);

            Vector3 battlePos = board.childNodeList[routePosition + 1].transform.position;
            battleFieldModel.transform.position = battlePos + new Vector3(0, 3, 0);
            battleFieldModel.transform.LookAt(new Vector3(transform.position.x, battleFieldModel.transform.position.y, transform.position.z));

            battleFieldModel.transform.DOMoveY(battlePos.y, .2f).OnComplete(() =>
            {
                battleModelParticle.transform.position = battleFieldModel.transform.position;
                battleModelParticle.Play();
                this.transform.DOLookAt(new Vector3(battleFieldModel.transform.position.x, transform.position.y, battleFieldModel.transform.position.z), .3f).
                OnComplete(() =>
                {
                    itemMark.SetActive(true);
                    itemMark.transform.DOMoveY(.3f, .2f).SetLoops(2, LoopType.Yoyo);
                }).SetDelay(1);
            });

            yield return new WaitForSeconds(5f);
            //BattleScene();
        }

        isMove = false;
        PlayerPrefs.SetInt("StageValue", routePosition);
        diceText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        isStart = false;
        isLock = false;
    }

    public EnemyType ReturnBtDifficult()
    {
        return board.boardList[routePosition].enemyType;
    }

    private bool MoveNextNode(Vector3 goal)
    {
        anim.SetBool("isMove", true);
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
        Global.LoadScene.LoadScene("Battle");
        StageManager.Instance.enemyType = ReturnBtDifficult();
    }



    public void BoomDice()
    {
        diceObj.SetActive(false);
        diceParticle.gameObject.SetActive(true);
        diceParticle.transform.position = diceObj.transform.position;
        diceParticle.Play();
        steps = 1;//UnityEngine.Random.Range(1, 7);
        diceText.gameObject.SetActive(true);
        diceText.transform.position = cam.WorldToScreenPoint(diceObj.transform.position);
        diceText.text = steps.ToString();

        if (!isMove)
        {
            if (routePosition + steps < board.childNodeList.Count)
            {
                StartCoroutine(MoveMentCo());
                print("Move");
            }
        }
    }




}
