using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class BattleCameraController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Vector3 camOriginPos;
    private Quaternion camOriginRot;
    private static BattleCameraController Instance;
    [SerializeField]
    private Transform focusPanelTop;
    [SerializeField]
    private Transform focusPanelBottom;
    [SerializeField]
    private TMP_Text enemyNameText;
    private Vector3 panelTopOriginPos;
    private Vector3 panelBottomOriginPos;

    [SerializeField]
    private Transform letterboxTop;
    [SerializeField]
    private Transform letterboxBottom;
    [SerializeField]
    private TMP_Text SubTextTMP;
    public UnityEngine.UI.Text dummyTxt;
    private Vector3 letterboxTopOriginPos;
    private Vector3 letterboxBottomOriginPos;
    private void Awake()
    {
        Instance = this;
        camOriginPos = cam.transform.position;
        camOriginRot = cam.transform.rotation;
        panelTopOriginPos = focusPanelTop.localPosition;
        panelBottomOriginPos = focusPanelBottom.localPosition;
        letterboxTopOriginPos = letterboxTop.localPosition;
        letterboxBottomOriginPos = letterboxBottom.localPosition;
    }
    public static IEnumerator ZoomInEnemy()
    {
        Debug.Log(EnemyManager.Instance);
        Instance.enemyNameText.text = CardManager.Instance.FindEnemyItem(EnemyManager.Instance.CurEnemyUid).itemName;
        Instance.enemyNameText.DOFade(0, 0);
        yield return ZoomIn(NewFieldManager.Instance.enemyCard.LinkedModel.ModelObject.transform);
        yield break;
    }
    public static IEnumerator PanelIn()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Instance.focusPanelTop.DOLocalMove(Vector3.zero, .7f));
        seq.Join(Instance.focusPanelBottom.DOLocalMove(Vector3.zero, .7f));
        seq.Join(Instance.enemyNameText.transform.DOLocalMoveX(Instance.enemyNameText.transform.localPosition.x - 100f, 0f));
        seq.Join(Instance.enemyNameText.transform.DOLocalMoveY(Instance.enemyNameText.transform.localPosition.y + 50f, 0f));
        seq.Append(Instance.enemyNameText.DOFade(1, .5f));
        seq.Join(Instance.enemyNameText.transform.DOLocalMoveX(Instance.enemyNameText.transform.localPosition.x + 100f, .5f));
        seq.Join(Instance.enemyNameText.transform.DOLocalMoveY(Instance.enemyNameText.transform.localPosition.y - 50f, .5f));
        seq.SetEase(Ease.InSine);

        yield return seq;
        yield break;
    }
    public static IEnumerator LetterBoxActive(bool isActive)
    {
        Sequence seq = DOTween.Sequence();
        if(isActive)
        {
            seq.Append(Instance.letterboxTop.DOLocalMove(Vector3.zero, .7f));
            seq.Join(Instance.letterboxBottom.DOLocalMove(Vector3.zero, .7f));
            yield return new WaitForSeconds(1.4f);
        }
        else
        {
            seq.Append(Instance.letterboxTop.DOLocalMove(Instance.letterboxTopOriginPos, .4f));
            seq.Join(Instance.letterboxBottom.DOLocalMove(Instance.letterboxBottomOriginPos, .4f));
            yield return new WaitForSeconds(.8f);
        }
        yield break;
    }
    public static IEnumerator SubText(string txt = "기본 텍스트",float duration = 0)
    {
        if (duration == 0)
        {
            duration = txt.Trim().Length * .5f;
        }
        Sequence seq = DOTween.Sequence();
        seq.Append(Instance.dummyTxt.DOText(txt, duration).OnUpdate(() => Instance.SubTextTMP.text = Instance.dummyTxt.text).OnComplete(() => { Instance.dummyTxt.text = ""; Instance.SubTextTMP.text = ""; }));
        seq.SetEase(Ease.InSine);
        yield return new WaitForSeconds(duration);
        yield break;
    }
    public static IEnumerator PanelOut()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Instance.focusPanelTop.DOLocalMove(Instance.panelTopOriginPos, .4f));
        seq.Join(Instance.focusPanelBottom.DOLocalMove(Instance.panelBottomOriginPos, .4f));
        seq.Append(Instance.enemyNameText.transform.DOLocalMoveX(Instance.enemyNameText.transform.localPosition.x - 100f, 0f));
        seq.Append(Instance.enemyNameText.transform.DOLocalMoveY(Instance.enemyNameText.transform.localPosition.y + 50f, 0f));
        seq.SetEase(Ease.InSine);

        yield return seq;
        yield break;
    }
    public static IEnumerator ZoomOut()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(Instance.focusPanelTop.DOLocalMove(Instance.panelTopOriginPos, .4f));
        seq.Join(Instance.focusPanelBottom.DOLocalMove(Instance.panelBottomOriginPos, .4f));
        seq.Append(Instance.cam.transform.DOMove(Instance.camOriginPos, 1f));
        seq.Join(Instance.cam.transform.DORotateQuaternion(Instance.camOriginRot, 1f));
        seq.SetEase(Ease.Linear);
        yield return seq;

        foreach (var card in CardManager.Instance.myCards)
        {
            card.DetactiveCardView(true);
        }
        yield break;
    }

    public static IEnumerator ZoomInPlayer()
    {
        yield return ZoomIn(NewFieldManager.Instance.playerCard.LinkedModel.ModelObject.transform);
        yield break;
    }
    public static IEnumerator ZoomIn(Transform focusedTrm)
    {
        foreach (var card in CardManager.Instance.myCards)
        {
            card.DetactiveCardView(false);
        }
        Vector3 camPos = focusedTrm.position;
        camPos += new Vector3(0f, 5f, -7f);
        Sequence seq = DOTween.Sequence();
        seq.Append(Instance.cam.transform.DOMove(camPos, 1.5f));
        seq.Join(Instance.cam.transform.DORotate((focusedTrm.position - camPos).normalized, 1.5f));
        seq.SetEase(Ease.InSine);
        yield return seq;
        yield break;
    }


}
