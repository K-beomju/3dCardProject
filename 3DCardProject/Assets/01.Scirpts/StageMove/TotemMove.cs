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
    private WaitForSeconds rollDelay = new WaitForSeconds(0.05f);
    private IEnumerator rollCo;
    [SerializeField] Camera cam;

    #endregion


    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        rollCo = RollingDice();
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
        if (isRoll)
        {
            if (Time.time > nextTime)
            {
                nextTime = Time.time + TimeLeft;
                bChRot = !bChRot;
            }

            if (bChRot)
            {
                diceObj.transform.Rotate(new Vector3(45, 45, 45) * rotSpeed * Time.deltaTime);
            }
            else
            {
                diceObj.transform.Rotate(new Vector3(-45, 45, -45) * rotSpeed * Time.deltaTime);

            }

        }

        if (!isMove)
            diceText.transform.position = new Vector3(cam.WorldToScreenPoint(transform.position).x, diceText.transform.position.y, cam.WorldToScreenPoint(transform.position).z);
        else
            diceText.transform.position = cam.WorldToScreenPoint(transform.position + new Vector3(0, 1.8f, 0));


        if (Input.GetKeyDown(KeyCode.Space) && !isLock)
        {
            if (!isRoll)
            {
                diceObj.transform.position = transform.position + new Vector3(0, 2.14f, 0);
                diceObj.SetActive(true);

                StartCoroutine(rollCo);
                isRoll = true;

            }
            else
            {
                float posY = transform.position.y;
                transform.DOMoveY(-0.1f, 0.4f).OnComplete(() =>
                {
                    isRoll = false;

                    switch (steps)
                    {
                        case 1:
                            diceObj.transform.localEulerAngles = new Vector3(0, 90, 0);
                            break;
                        case 2:
                            diceObj.transform.localEulerAngles = new Vector3(90, 90, 0);
                            break;
                        case 3:
                            diceObj.transform.localEulerAngles = new Vector3(90, 180, 0);
                            break;
                        case 4:
                            diceObj.transform.localEulerAngles = Vector3.zero;
                            break;
                        case 5:
                            diceObj.transform.localEulerAngles = new Vector3(0, -180, -90);
                            break;
                        case 6:
                            diceObj.transform.localEulerAngles = new Vector3(0, -90, 0);
                            break;
                        default:
                            print("Dice NULL");
                            break;
                    }
                    diceObj.transform.DOMoveY(1.8f, 0.3f).OnComplete(() => diceObj.transform.DOMoveY(1.47f, 0.3f).OnComplete(() =>
                    {
                        if (!isMove)
                        {
                            if (routePosition + steps < board.childNodeList.Count)
                            {
                                StartCoroutine(MoveMentCo());
                                print("Move");
                            }
                        }
                    }));
                    transform.DOMoveY(posY, 0.4f);

                });
                StopCoroutine(rollCo);

            }
        }
    }

    private IEnumerator MoveMentCo()
    {
        yield return new WaitForSeconds(0.5f);

        diceParticle.Play();
        diceParticle.transform.position = diceObj.transform.position;
        diceObj.SetActive(false);
        diceText.text = steps.ToString();
        diceText.gameObject.SetActive(true);
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

        //if(board.boardList[routePosition].type == StageType.Battle) // 만약 도착한 노드의 타입이 배틀이라면
        //    FadeInOut(FadeType.FadeOut, BattleScene);

        isMove = false;
        PlayerPrefs.SetInt("StageValue", routePosition);
        diceText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);

        isLock = false;
    }

    private IEnumerator RollingDice()
    {
        while (true)
        {
            steps = UnityEngine.Random.Range(1, 7);

            yield return rollDelay;
        }
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
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.gameObject.SetActive(false);
        }
    }






}
