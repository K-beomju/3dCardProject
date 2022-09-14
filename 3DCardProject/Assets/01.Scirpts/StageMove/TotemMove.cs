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
    [SerializeField]
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
        isTutorial = TutorialManager.Instance != null && TutorialManager.Instance.isTutorial;
        isLock = isTutorial;
        stageValue = PlayerPrefs.GetInt("StageValue");
        routePosition = stageValue;
        transform.position = board.childNodeList[stageValue].transform.position;
        diceObj.transform.DOMoveY(-0.1f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(ease);
        battleFieldModel.SetActive(false);
        itemMark.SetActive(false);
        if(isTutorial)
        {
            tutorialValue = SecurityPlayerPrefs.GetInt("TutorialValue",0);
            StartCoroutine(TutorialCol());
        }
    }

    private float TimeLeft = 1.0f;
    private float nextTime = 0.0f;
    private bool bChRot = false;
    
    [SerializeField]
    private bool isTutorial;
    private int tutorialValue = 0;

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StopAllCoroutines();
            Global.LoadScene.LoadScene("Stage");
        }
        if (isMove)
            diceText.transform.position = cam.WorldToScreenPoint(transform.position + new Vector3(0, 1.2f, 0));

    }
    [ContextMenu("ResetStageValue")]
    public void ResetStageValue()
    {
        SecurityPlayerPrefs.DeleteKey("TutorialValue");
    }
    private IEnumerator TutorialCol()
    {
        switch (tutorialValue)
        {
            case 0:
                yield return TutorialManager.Instance.ExplainCol("스테이지에 관한 튜토리얼 입니다.", 0);
                yield return TutorialManager.Instance.ExplainCol("\"Space\"를(을) 눌러 주사위를 굴러보세요.", 250);
                isLock = false;
                yield return Utils.WaitForInputKey(KeyCode.Space);
                yield return TutorialManager.Instance.ExplainCol("\"Space\"를(을) 한번 더 눌러 이동합니다.", 250);
                break;
            case 1:
            case 2:
                yield return TutorialManager.Instance.ExplainCol("\"Space\"를(을) 눌러 주사위를 굴러보세요.", 250);
                isLock = false;
                yield return Utils.WaitForInputKey(KeyCode.Space);
                yield return TutorialManager.Instance.ExplainCol("\"Space\"를(을) 한번 더 눌러 이동합니다.", 250);
                break;
            case 3:
                yield return TutorialManager.Instance.ExplainCol("잘 하셨습니다.", 250);
                yield return TutorialManager.Instance.ExplainCol("튜토리얼은 여기까지 입니다.", 250);
                PlayerPrefs.SetInt("StageValue",0);
                Global.LoadScene.LoadScene("Stage");
                yield break;
            default:
                break;
        }
        tutorialValue++;
        SecurityPlayerPrefs.SetInt("TutorialValue", tutorialValue);
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

        if (type == StageType.Battle) // 만약 도착한 노드의 타입이 배틀이라면
        {

            Vector3 lookAtPos = new Vector3(transform.position.x, battleFieldModel.transform.position.y, transform.position.z);

            battleFieldModel.SetActive(true);

            Vector3 battlePos = board.childNodeList[routePosition + 1].transform.position;
            battleFieldModel.transform.position = battlePos + new Vector3(0, 3, 0);
            battleFieldModel.transform.LookAt(new Vector3(transform.position.x, battleFieldModel.transform.position.y, transform.position.z));

            battleFieldModel.transform.DOMoveY(battlePos.y, .2f).OnComplete(() =>
            {
                battleModelParticle.gameObject.SetActive(true);
                battleModelParticle.transform.position = battleFieldModel.transform.position + new Vector3(0,0.2f,0);
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
            this.transform.DOLookAt(new Vector3(necro.transform.position.x, transform.position.y, necro.transform.position.z), .3f);

            yield return new WaitForSeconds(2f);
            necro.MegaPt(this.transform);
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
        return board.boardList[routePosition/2].enemyType;
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
        if (SceneManager.GetActiveScene().name == "Stage" || SceneManager.GetActiveScene().name == "Tutorials")
        {
            diceObj.SetActive(false);
            diceParticle.gameObject.SetActive(true);
            diceParticle.transform.position = diceObj.transform.position;
            diceParticle.Play();
            if (isTutorial)
            {
                steps = 1;
            }
            else
            {
                steps = UnityEngine.Random.Range(1, 7);
            }
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
