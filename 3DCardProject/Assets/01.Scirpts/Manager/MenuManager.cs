using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : Singleton<MenuManager>
{
    // 타이틀 이동, 게임 나가기, 사운드 관련 기능은 추가 후에 
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Text selectText;

    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button moveTitleBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button descBtn;
    [SerializeField] private Button descExitBtn;

    [SerializeField] private CanvasGroup middleGroup;
    [SerializeField] private CanvasGroup gameDescGroup;
    [SerializeField] private CanvasGroup buttonGroup;


    private RectTransform resumeTrm;
    private RectTransform moveTitleTrm;
    private RectTransform exitTrm;
    private RectTransform descTrm;

    private bool isMenuActive = false;


    protected override void Awake()
    {
        base.Awake();
        resumeBtn.onClick.AddListener(() => menuPanel.SetActive(true));
        exitBtn.onClick.AddListener(() => ExitGame());
        moveTitleBtn.onClick.AddListener(() => MoveTitle());
        descBtn.onClick.AddListener(() => DescGameRule());
        descExitBtn.onClick.AddListener(() => DescDetactive());

        resumeTrm = resumeBtn.GetComponent<RectTransform>();
        moveTitleTrm = moveTitleBtn.GetComponent<RectTransform>();
        exitTrm = exitBtn.GetComponent<RectTransform>();
        descTrm = descBtn.GetComponent<RectTransform>();
       
    }

    private void Start()
    {
        menuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActive = !isMenuActive;

            menuPanel.SetActive(isMenuActive);
            if (isMenuActive)
            {
                Sequence mySeq = DOTween.Sequence();
                mySeq.Append(descTrm.DOAnchorPosX(375, .4f).SetEase(Ease.Linear))
                .Insert(0.1f,exitTrm.DOAnchorPosX(125, .4f).SetEase(Ease.Linear))
                .Insert(0.2f, moveTitleTrm.DOAnchorPosX(-125, .4f).SetEase(Ease.Linear))
                .Insert(0.3f, resumeTrm.DOAnchorPosX(-375, .4f).SetEase(Ease.Linear));
                mySeq.Play();
            }
            else
            {
                ResetButtonTransform();
            }

        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }


    public void MoveTitle()
    {
        Global.LoadScene.LoadScene("Title",() => menuPanel.SetActive(false));
    }
        
    public void DescGameRule()
    {
        middleGroup.DOFade(0, 1).OnComplete(() => gameDescGroup.DOFade(1,1));
        buttonGroup.DOFade(0, 1).OnComplete(() => buttonGroup.interactable = false);
        middleGroup.blocksRaycasts = false;
        buttonGroup.blocksRaycasts = false;
        gameDescGroup.interactable = true;
        gameDescGroup.blocksRaycasts = true;
    }

    public void DescDetactive()
    {
        gameDescGroup.DOFade(0, 1).OnComplete(() => middleGroup.DOFade(1, 1));
        buttonGroup.DOFade(1, 1).OnComplete(() => buttonGroup.interactable = true);
        middleGroup.blocksRaycasts = true;
        buttonGroup.blocksRaycasts = true;
        gameDescGroup.interactable = false;
        gameDescGroup.blocksRaycasts = false;

    }

    public void SelectButton(string name)
    {
        selectText.text = "- " + name + " -";
    }

    public void ResetButtonTransform()
    {
        resumeTrm.anchoredPosition = new Vector2(-1860, 65);
        moveTitleTrm.anchoredPosition = new Vector2(-1610, 65);
        exitTrm.anchoredPosition = new Vector2(-1360, 65);
        descTrm.anchoredPosition = new Vector2(-1110, 65);

    }

}
