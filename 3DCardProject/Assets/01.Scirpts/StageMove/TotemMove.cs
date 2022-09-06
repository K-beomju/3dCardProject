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
    [SerializeField] private ParticleSystem healParticle;
    [SerializeField] private ParticleSystem healFieldParticle;
    [SerializeField] private GameObject itemMark;
    [SerializeField] private Camera cam;

    public Necromancer necro;

    public int routePosition;
    public int stageValue;
    public int routeMinus;

    private bool isMove = false;
    private bool isLock = false;
    private bool isStart = false;
    private Animator anim;

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
                diceObj.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), .5f, 2);
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
        diceParticle.gameObject.SetActive(false);
        healParticle.gameObject.SetActive(false);

        while (steps > 0)
        {
            Vector3 nextPos = board.childNodeList[routePosition + 1].transform.position;
            Vector3 lookAtPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
            transform.LookAt(lookAtPos);

            while (MoveNextNode(nextPos))
                yield return null;

            // ���� Ŭ���� �˻� 
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

        var type = board.boardList[routePosition - routeMinus].type;

        if (type == StageType.Battle) // ���� ������ ����� Ÿ���� ��Ʋ�̶��
        {

            Vector3 lookAtPos = new Vector3(transform.position.x, battleFieldModel.transform.position.y, transform.position.z);

            battleFieldModel.SetActive(true);

            Vector3 battlePos = board.childNodeList[routePosition + 1].transform.position;
            battleFieldModel.transform.position = battlePos + new Vector3(0, 3, 0);
            battleFieldModel.transform.LookAt(new Vector3(transform.position.x, battleFieldModel.transform.position.y, transform.position.z));

            battleFieldModel.transform.DOMoveY(battlePos.y, .2f).OnComplete(() =>
            {
                battleModelParticle.gameObject.SetActive(true);
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
            BattleScene();
        }
        if (type == StageType.Shop)
        {
            Vector3 battlePos = board.childNodeList[routePosition + 1].transform.position;
            necro.gameObject.SetActive(true);
            necro.transform.position = battlePos + new Vector3(0, .3f, 0);
            necro.transform.LookAt(new Vector3(transform.position.x, necro.transform.position.y, transform.position.z));
            necro.transform.Rotate(10, 0, 0);

            yield return new WaitForSeconds(2f);
            necro.MegaPt();
            yield return new WaitForSeconds(1f);

            ShopScene();
        }
        if (type == StageType.Rest)
        {
            healFieldParticle.gameObject.SetActive(true);
            healFieldParticle.transform.position = transform.position;
            healFieldParticle.Play();
            yield return new WaitForSeconds(2f);
            FadeInOut(1, 1, () => RestAction());
        }

        isMove = false;
        PlayerPrefs.SetInt("StageValue", routePosition);
        diceText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        routeMinus = 0;
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


    private void FadeInOut(int time, int delayTime, Action action = null)
    {
        fadeGroup.DOFade(1, time).OnComplete(() => fadeGroup.DOFade(0,time).OnComplete(() => action()).SetDelay(delayTime));
    }

    private void BattleScene()
    {
        Global.LoadScene.LoadScene("Battle", () => { StageManager.Instance.OnLoadBattleScene?.Invoke(); StageManager.Instance.SceneState = SceneState.BATTLE; });

        StageManager.Instance.enemyType = ReturnBtDifficult();
    }

    private void ShopScene()
    {
        Global.LoadScene.LoadScene("Shop", () => { StageManager.Instance.OnLoadShopScene?.Invoke(); StageManager.Instance.SceneState = SceneState.SHOP; });

    }

    public void BoomDice()
    {
        if (SceneManager.GetActiveScene().name == "Stage")
        {
            diceObj.SetActive(false);
            diceParticle.gameObject.SetActive(true);
            diceParticle.transform.position = diceObj.transform.position;
            diceParticle.Play();
            steps = UnityEngine.Random.Range(1, 7);
            routeMinus = steps;
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

    public void RestAction()
    {
        healParticle.gameObject.SetActive(true);
        healParticle.transform.position = transform.position + new Vector3(0,0.5f,0);
        healParticle.Play();
        healFieldParticle.gameObject.SetActive(false);
    }



}
