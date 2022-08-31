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
    [SerializeField] private int steps;
    [SerializeField] private CanvasGroup fadeGroup;

    private bool isMove = false;
    private bool isLock = false;
    public enum FadeType
    {
        FadeIn,
        FadeOut
    }
    private FadeType fadeType;

    public int routePosition;
    public int stageValue;
    private Animator anim;

    #region Dice
    public Text diceText;

    public float rotSpeed;
    [SerializeField] private GameObject diceObj;
    [SerializeField] private ParticleSystem diceParticle;
    private bool isRoll = false;
    [SerializeField] Camera cam;

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
    }

    private float TimeLeft = 1.0f;
    private float nextTime = 0.0f;
    private bool bChRot = false;
    private void Update()
    {
   
        if (Input.GetKeyDown(KeyCode.Space) && !isLock)
        {
            if (!isRoll)
            {
                anim.SetTrigger("Attack");
                isRoll = true;
            }
            else
            {

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
    }

    private IEnumerator MoveMentCo()
    {
        yield return new WaitForSeconds(0.5f);     
        isLock = true;
        yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(0.1f);
            steps--;
            diceText.text = steps.ToString();

            routePosition++;
            board.ChangeCam();

        }
        anim.SetBool("isMove", false);
        yield return new WaitForSeconds(0.5f);

        if (board.boardList[routePosition].type == StageType.Battle) // 만약 도착한 노드의 타입이 배틀이라면
            FadeInOut(FadeType.FadeOut, BattleScene);

        isMove = false;
        PlayerPrefs.SetInt("StageValue", routePosition);
        diceText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);

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
        SceneManager.LoadScene("MinSangSang");
        StageManager.Instance.enemyType = ReturnBtDifficult();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.gameObject.SetActive(false);
        }
    }

    public void BoomDice()
    {
        diceObj.SetActive(false);
        diceParticle.transform.position = diceObj.transform.position;
        diceParticle.Play();
        steps = UnityEngine.Random.Range(1, 7);
        diceText.gameObject.SetActive(true);
        diceText.transform.position = cam.WorldToScreenPoint(diceObj.transform.position);
        diceText.text = steps.ToString();
    }




}
