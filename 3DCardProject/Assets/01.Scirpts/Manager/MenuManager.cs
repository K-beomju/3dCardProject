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
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button soundExitBtn;

    [SerializeField] private CanvasGroup middleGroup;
    [SerializeField] private CanvasGroup gameDescGroup;
    [SerializeField] private CanvasGroup buttonGroup;
    [SerializeField] private CanvasGroup soundGroup;


    private RectTransform resumeTrm;
    private RectTransform moveTitleTrm;
    private RectTransform exitTrm;
    private RectTransform descTrm;
    private RectTransform soundTrm;

    private bool isMenuActive = false;


    protected override void Awake()
    {
        base.Awake();
        resumeBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayFXSound("ClickButton", 0.2f);

            menuPanel.SetActive(false);
            Time.timeScale = 1;

        });
        exitBtn.onClick.AddListener(() => ExitGame());
        moveTitleBtn.onClick.AddListener(() => MoveTitle());
        descBtn.onClick.AddListener(() => DescGameRule());
        soundBtn.onClick.AddListener(() => SetSoundPanel());

        descExitBtn.onClick.AddListener(() => DescDetactive());
        soundExitBtn.onClick.AddListener(() => DetactiveSoundPanel());

        resumeTrm = resumeBtn.GetComponent<RectTransform>();
        moveTitleTrm = moveTitleBtn.GetComponent<RectTransform>();
        exitTrm = exitBtn.GetComponent<RectTransform>();
        descTrm = descBtn.GetComponent<RectTransform>();
        soundTrm = soundBtn.GetComponent<RectTransform>();

    }

    private void Start()
    {
        menuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorial) return;

            isMenuActive = !isMenuActive;

            menuPanel.SetActive(isMenuActive);
            if (isMenuActive)
            {
                Time.timeScale = 0;
                Sequence mySeq = DOTween.Sequence();
                mySeq.Append(soundTrm.DOAnchorPosX(500, .4f).SetEase(Ease.Linear))
                .Insert(0.1f, descTrm.DOAnchorPosX(250, .4f).SetEase(Ease.Linear))
                .Insert(0.2f, exitTrm.DOAnchorPosX(0, .4f).SetEase(Ease.Linear))
                .Insert(0.3f, moveTitleTrm.DOAnchorPosX(-250, .4f).SetEase(Ease.Linear))
                .Insert(0.4f, resumeTrm.DOAnchorPosX(-500, .4f).SetEase(Ease.Linear));
                mySeq.Play().SetUpdate(true);
            }
            else
            {
                Time.timeScale = 1;
                ResetButtonTransform();
            }

        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        SaveManager.Instance.NewGame();
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        SaveManager.Instance.NewGame();
        Application.Quit();
    }


    public void MoveTitle()
    {
        SoundManager.Instance.PlayFXSound("ClickButton", 0.2f);
        Time.timeScale = 1;
        Global.LoadScene.LoadScene("Title", () => menuPanel.SetActive(false));
    }

    public void DescGameRule()
    {
        SoundManager.Instance.PlayFXSound("ClickButton", 0.2f);

        middleGroup.DOFade(0, 1).SetUpdate(true).OnComplete(() => gameDescGroup.DOFade(1, 1).SetUpdate(true));
        buttonGroup.DOFade(0, 1).SetUpdate(true).OnComplete(() => buttonGroup.interactable = false);
        middleGroup.blocksRaycasts = false;
        buttonGroup.blocksRaycasts = false;
        gameDescGroup.interactable = true;
        gameDescGroup.blocksRaycasts = true;
        Debug.Log(middleGroup.blocksRaycasts);
    }

    public void DescDetactive()
    {
        SoundManager.Instance.PlayFXSound("ClickButton", 0.2f);

        gameDescGroup.DOFade(0, 1).SetUpdate(true).OnComplete(() => middleGroup.DOFade(1, 1).SetUpdate(true));
        buttonGroup.DOFade(1, 1).SetUpdate(true).OnComplete(() => buttonGroup.interactable = true);
        middleGroup.blocksRaycasts = true;
        buttonGroup.blocksRaycasts = true;
        gameDescGroup.interactable = false;
        gameDescGroup.blocksRaycasts = false;

    }

    public void SetSoundPanel()
    {
        SoundManager.Instance.PlayFXSound("ClickButton", 0.2f);

        middleGroup.DOFade(0, 1).SetUpdate(true).OnComplete(() => soundGroup.DOFade(1, 1).SetUpdate(true));
        buttonGroup.DOFade(0, 1).SetUpdate(true).OnComplete(() => buttonGroup.interactable = false);
        middleGroup.blocksRaycasts = false;
        buttonGroup.blocksRaycasts = false;
        soundGroup.interactable = true;
        soundGroup.blocksRaycasts = true;
    }

    public void DetactiveSoundPanel()
    {
        SoundManager.Instance.PlayFXSound("ClickButton", 0.2f);

        Debug.Log(soundGroup);
        soundGroup.DOFade(0, 1).SetUpdate(true).OnComplete(() => middleGroup.DOFade(1, 1).SetUpdate(true));
        buttonGroup.DOFade(1, 1).SetUpdate(true).OnComplete(() => buttonGroup.interactable = true);
        middleGroup.blocksRaycasts = true;
        buttonGroup.blocksRaycasts = true;
        soundGroup.interactable = false;
        soundGroup.blocksRaycasts = false;
    }

    public void SelectButton(string name)
    {
        selectText.text = "- " + name + " -";
    }

    public void ResetButtonTransform()
    {
        resumeTrm.anchoredPosition = new Vector2(-2100, 65);
        moveTitleTrm.anchoredPosition = new Vector2(-1850, 65);
        exitTrm.anchoredPosition = new Vector2(-1600, 65);
        descTrm.anchoredPosition = new Vector2(-1350, 65);
        soundTrm.anchoredPosition = new Vector2(-1100, 65);
    }

}
