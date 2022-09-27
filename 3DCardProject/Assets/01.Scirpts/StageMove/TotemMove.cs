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
    [SerializeField] private PlayerDataInfo playerData;

    [SerializeField] private float speed;
    [SerializeField] private int steps;
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private GameObject diceObj;
    [SerializeField] private Transform diceTrm;
    [SerializeField] private ParticleSystem diceParticle;
    [SerializeField] private ParticleSystem battleModelParticle;
    [SerializeField] private ParticleSystem healParticle;
    [SerializeField] private GameObject itemMark;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject rock;
    [SerializeField] private ParticleSystem rockParticle;
    [SerializeField] private Material baseMat;

    public Necromancer necro;

    public int routePosition;   // �̵��Ÿ� ���� (�̺�Ʈ + �̵� �ʵ�)
    public int routeStep;       // �ֻ��� �� ����                     


    private bool isMove = false;
    [SerializeField]
    private bool isLock = false;
    private bool isStart = false;
    public bool isCross = false;
    private Animator anim;


    [SerializeField] private CanvasGroup spaceGroup;

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
        routePosition = SaveManager.Instance.gameData.StageValue + SaveManager.Instance.gameData.RouteValue;

        Vector3 nextPos = board.childNodeList[routePosition + 2].transform.position;
        Vector3 lookAtPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
        StartCoroutine(LookAtCo(lookAtPos));

        transform.position = board.boardList[SaveManager.Instance.gameData.StageValue].transform.position + new Vector3(0, 0.1f, 0);
        diceObj.transform.position = diceTrm.position;
        battleFieldModel.SetActive(false);
        itemMark.SetActive(false);
        rotSpeed = 0;
        if (spaceGroup != null)
        {
            spaceGroup.gameObject.SetActive(true);
            spaceGroup.DOFade(0.7f, 1).SetLoops(-1, LoopType.Yoyo);
        }

        board.CheckRouteCam(false);

        if (isTutorial)
        {
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
        diceObj.transform.Rotate(new Vector3(30, 0, 30) * rotSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) && !isLock)
        {
            if (!isStart)
            {
                SoundManager.Instance.PlayFXSound("SpawnDice", 0.1f);

                diceObj.SetActive(true);
                diceObj.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), .5f, 2);
                isStart = true;
                DOTween.To(() => rotSpeed, x => rotSpeed = x, 20, 1);
            }
            else
            {
                if (spaceGroup != null)
                {
                    spaceGroup.DOFade(0, 1).OnComplete(() => spaceGroup.gameObject.SetActive(false));
                }
                anim.SetTrigger("Attack");
                isLock = true;
            }
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
                yield return TutorialManager.Instance.ExplainCol("���������� ���� Ʃ�丮�� �Դϴ�.", 0);
                yield return TutorialManager.Instance.ExplainCol("\"Space\"��(��) ���� �ֻ����� ����������.", 250);
                isLock = false;
                yield return Utils.WaitForInputKey(KeyCode.Space);
                yield return TutorialManager.Instance.ExplainCol("\"Space\"��(��) �ѹ� �� ���� �̵��մϴ�.", 250);
                break;
            case 1:
                yield return TutorialManager.Instance.ExplainCol("\"Space\"��(��) ���� �ֻ����� ����������.", 250);
                isLock = false;
                yield return Utils.WaitForInputKey(KeyCode.Space);
                yield return TutorialManager.Instance.ExplainCol("\"Space\"��(��) �ѹ� �� ���� �̵��մϴ�.", 250);
                break;
            case 2:
                yield return TutorialManager.Instance.ExplainCol("�� �ϼ̽��ϴ�.", 250);
                yield return TutorialManager.Instance.ExplainCol("Ʃ�丮���� ������� �Դϴ�.", 250);
                TutorialManager.Instance.isTutorial = false;
                SaveManager.Instance.gameData.IsTutorialDone = true;
                StageManager.Instance.SceneState = SceneState.STAGE;
                Global.LoadScene.LoadScene("Stage");
                yield break;
            default:
                break;
        }
        tutorialValue++;
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

            if (board.boardList[routePosition / 2].isCross && !board.isEndCross) // �����濡 �����Ͽ��°�? 
            {
                isCross = true;
                StartCoroutine(MoveTotemCO());
                yield return new WaitUntil(() => !isCross);
            }

            Vector3 nextPos = board.childNodeList[routePosition + 1].transform.position;
            Vector3 lookAtPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
            transform.DOLookAt(lookAtPos, .3f);

            while (MoveNextNode(nextPos))
            {
                yield return null;
            }


            if (board.childNodeList[routePosition].GetSiblingIndex() % 2 == 1)
            {
                steps--;
                diceText.text = steps.ToString();
            }
            routePosition++;
            board.CheckRouteCam(true);

            SaveManager.Instance.gameData.RouteValue = routePosition - routeStep - SaveManager.Instance.gameData.StageValue;

        }


        anim.SetBool("isMove", false);
        yield return new WaitForSeconds(2.5f);
        rotSpeed = 0;

        diceText.gameObject.SetActive(false);
        var type = board.boardList[SaveManager.Instance.gameData.RouteValue].type;

        //if (type == StageType.Battle) // ���� ������ ����� Ÿ���� ��Ʋ�̶��
        //{

        //    Vector3 lookAtPos = new Vector3(transform.position.x, battleFieldModel.transform.position.y, transform.position.z);

        //    battleFieldModel.SetActive(true);

        //    Vector3 battlePos = board.childNodeList[routePosition + 1].transform.position;
        //    battleFieldModel.transform.position = battlePos + new Vector3(0, 3f, 0);
        //    battleFieldModel.transform.LookAt(new Vector3(transform.position.x, battleFieldModel.transform.position.y, transform.position.z));

        //    battleFieldModel.transform.DOMoveY(battlePos.y + 0.1f, .2f).OnComplete(() =>
        //    {
        //        SoundManager.Instance.PlayFXSound("BattleTotem", 0.1f);
        //        if (playerData != null)
        //            playerData.ShowTopPanel("�Թ�Ʋ ����!");

        //        battleModelParticle.gameObject.SetActive(true);
        //        battleModelParticle.transform.position = battleFieldModel.transform.position + new Vector3(0, 0.2f, 0);
        //        battleModelParticle.Play();
        //        this.transform.DOLookAt(new Vector3(battleFieldModel.transform.position.x, transform.position.y, battleFieldModel.transform.position.z), .5f).
        //        OnComplete(() =>
        //        {
        //            SoundManager.Instance.PlayFXSound("Dangerous", 0.1f);

        //            itemMark.SetActive(true);
        //            itemMark.transform.DOMoveY(.5f, .2f).SetLoops(2, LoopType.Yoyo);
        //        }).SetDelay(1);
        //    });

        //    yield return new WaitForSeconds(4f);
        //    BattleScene();
        //}
        //if (type == StageType.Shop)
        //{
        //    if (playerData != null)
        //        playerData.ShowTopPanel("�Ի��� �̵�!");
        //    Vector3 battlePos = board.childNodeList[routePosition + 1].transform.position;
        //    necro.gameObject.SetActive(true);
        //    necro.transform.position = battlePos + new Vector3(0, .3f, 0);
        //    necro.transform.LookAt(new Vector3(transform.position.x, necro.transform.position.y, transform.position.z));
        //    necro.transform.Rotate(10, 0, 0);
        //    this.transform.DOLookAt(new Vector3(necro.transform.position.x, transform.position.y, necro.transform.position.z), .3f);

        //    yield return new WaitForSeconds(2f);
        //    necro.MegaPt(this.transform);
        //    yield return new WaitForSeconds(1f);

        //    ShopScene();
        //}
        //if (type == StageType.GetHP)
        //{
        //    playerData.ShowTopPanel("��ü�� ȸ��!");
        //    yield return new WaitForSeconds(3f);

        //    FadeInOut(1, 1, () => RestAction());
        //}
        //if (type == StageType.GetGold)
        //{
        //    playerData.ShowTopPanel("�԰�� ȹ��!");
        //    yield return new WaitForSeconds(3f);

        //    playerData.GetGoldIncreaseDirect();
        //}
        //if (type == StageType.LossGold)
        //{
        //    playerData.ShowTopPanel("�԰�� ����!");
        //    yield return new WaitForSeconds(3f);

        //    playerData.GetGoldDecreaseDirect();
        //}
        //if (type == StageType.LossHp)
        //{
        //    playerData.ShowTopPanel("��ü�� ����!");
        //    yield return new WaitForSeconds(3f);
        //    rock.transform.position = transform.position + new Vector3(0, 4, 0);
        //    rock.SetActive(true);

        //    Sequence mySeq = DOTween.Sequence();
        //    mySeq.Append(rock.transform.DOMoveY(transform.position.y, 0.3f));
        //    mySeq.InsertCallback(0.3f, () =>
        //    {
        //        rockParticle.gameObject.SetActive(true);
        //        rockParticle.transform.position = rock.transform.position;
        //        rockParticle.Play();
        //        rock.SetActive(false);
        //    }).InsertCallback(.3f, () => anim.SetTrigger("Dead"))
        //    .InsertCallback(.3f, () => baseMat.DOColor(Color.red, 0).OnComplete(() => baseMat.DOColor(Color.white, 1)))
        //    .InsertCallback(1f, () => playerData.GetHpDecreaseDirect(-3));




        //    mySeq.Play();

        //}

        isMove = false;
        SaveManager.Instance.gameData.StageValue = SaveManager.Instance.gameData.RouteValue;
        diceText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        isStart = false;
        isLock = false;
    }

    private IEnumerator MoveTotemCO()
    {
        Vector3 nextPos = board.childNodeList[routePosition + 1].transform.position;
        Vector3 lookAtPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
        transform.DOLookAt(lookAtPos, .3f);

        while (MoveNextNode(nextPos))
            yield return null;

        ++routePosition;
        anim.SetBool("isMove", false);
        board.SelectCrossRoadCamera();
    }


    public uint GetEnemyUid()
    {
        return board.boardList[routePosition / 2].uid;
    }
    public EnemyType ReturnBtDifficult()
    {
        return board.boardList[routePosition / 2].enemyType;
    }

    private bool MoveNextNode(Vector3 goal)
    {
        anim.SetBool("isMove", true);
        return goal + new Vector3(0, 0.1f, 0) != (transform.position = Vector3.MoveTowards(transform.position, goal + new Vector3(0, 0.1f, 0), speed * Time.deltaTime));
    }


    private void FadeInOut(int time, int delayTime, Action action = null)
    {
        fadeGroup.DOFade(1, time).OnComplete(() => fadeGroup.DOFade(0, time).OnComplete(() => action()).SetDelay(delayTime));
    }

    private void BattleScene()
    {

        EnemyManager.Instance.curEnemyType = ReturnBtDifficult();
        EnemyManager.Instance.CurEnemyUid = GetEnemyUid();

        Global.LoadScene.LoadScene("Battle", () => { StageManager.Instance.OnLoadBattleScene?.Invoke(); StageManager.Instance.SceneState = SceneState.BATTLE; });

        SoundManager.Instance.bgmVolume = 0.1f;
        SoundManager.Instance.PlayBGMSound("Battle",3);
    }

    private void ShopScene()
    {
        Global.LoadScene.LoadScene("Shop", () => { StageManager.Instance.OnLoadShopScene?.Invoke(); StageManager.Instance.SceneState = SceneState.SHOP; });

    }

    public void BoomDice()
    {
        if (SceneManager.GetActiveScene().name == "Stage" || SceneManager.GetActiveScene().name == "Tutorials")
        {
            SoundManager.Instance.PlayFXSound("AttackDice", 0.1f);
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
                int stepValue = (((board.boardList.Count - 1) * 2) - routePosition) / 2;
                if (stepValue != 0 && board.isEndCross)
                {
                    if (steps > stepValue)
                    {
                        steps = stepValue;
                    }
                }

            }

            if (!isMove)
            {
                if (routePosition + steps < board.childNodeList.Count)
                {
                    StartCoroutine(MoveMentCo());
                }
            }
            diceText.gameObject.SetActive(true);
            diceText.transform.position = cam.WorldToScreenPoint(diceObj.transform.position);
            diceText.text = steps.ToString();
            routeStep = steps;

        }

    }

    public void RestAction()
    {
        healParticle.gameObject.SetActive(true);
        healParticle.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        healParticle.Play();
        ++SaveManager.Instance.gameData.Hp;
        playerData.DataInfoScreen();
        if (spaceGroup != null)
        {
            spaceGroup.gameObject.SetActive(true);
            spaceGroup.DOFade(0.7f, 1).SetLoops(-1, LoopType.Yoyo);
        }

    }


    private IEnumerator LookAtCo(Vector3 pos)
    {
        yield return null;
        transform.LookAt(pos);

    }

    public void LookAtBoard()
    {
        Vector3 nextPoss = board.childNodeList[routePosition + 1].transform.position;
        Vector3 lookAtPoss = new Vector3(nextPoss.x, transform.position.y, nextPoss.z);
        transform.DOLookAt(lookAtPoss, 1);
    }


}
