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
    private void Awake()
    {
        Instance = this;
        camOriginPos = cam.transform.position;
        camOriginRot = cam.transform.rotation;
        panelTopOriginPos = focusPanelTop.localPosition;
        panelBottomOriginPos = focusPanelBottom.localPosition;
    }
    [ContextMenu("Focus")]
    public static IEnumerator FocusOnEnemy()
    {
        Instance.enemyNameText.text = CardManager.Instance.FindEnemyData(EnemyManager.Instance.CurEnemyUid).itemName;
        Instance.enemyNameText.DOFade(0, 0);
        Vector3 camPos = NewFieldManager.Instance.enemyCard.transform.position;
        camPos += new Vector3(0f,5f,-7f);
        Sequence seq = DOTween.Sequence();
        seq.Append(Instance.cam.transform.DOMove(camPos, 1.5f));
        seq.Join(Instance.cam.transform.DORotate((NewFieldManager.Instance.enemyCard.LinkedModel.transform.position - camPos).normalized, 1.5f));
        seq.SetEase(Ease.InSine);
        seq.Append(Instance.focusPanelTop.DOLocalMove(Vector3.zero,.7f));
        seq.Join(Instance.focusPanelBottom.DOLocalMove(Vector3.zero, .7f));
        seq.Join(Instance.enemyNameText.transform.DOLocalMoveX(Instance.enemyNameText.transform.localPosition.x - 100f,0f));
        seq.Join(Instance.enemyNameText.transform.DOLocalMoveY(Instance.enemyNameText.transform.localPosition.y + 50f,0f));
        seq.Append(Instance.enemyNameText.DOFade(1, .5f));
        seq.Join(Instance.enemyNameText.transform.DOLocalMoveX(Instance.enemyNameText.transform.localPosition.x + 100f, .5f));
        seq.Join(Instance.enemyNameText.transform.DOLocalMoveY(Instance.enemyNameText.transform.localPosition.y - 50f, .5f));
        seq.AppendInterval(5f);
        seq.Append(Instance.focusPanelTop.DOLocalMove(Instance.panelTopOriginPos, .4f));
        seq.Join(Instance.focusPanelBottom.DOLocalMove(Instance.panelBottomOriginPos, .4f));
        seq.Append(Instance.enemyNameText.transform.DOLocalMoveX(Instance.enemyNameText.transform.localPosition.x - 100f, 0f));
        seq.Append(Instance.enemyNameText.transform.DOLocalMoveY(Instance.enemyNameText.transform.localPosition.y + 50f, 0f));
        yield return new WaitForSeconds(5.2f);
        yield break;
    }
    [ContextMenu("OutFocus")]
    public static IEnumerator OutFocusFromEnemy()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(Instance.focusPanelTop.DOLocalMove(Instance.panelTopOriginPos, .4f));
        seq.Join(Instance.focusPanelBottom.DOLocalMove(Instance.panelBottomOriginPos, .4f));
        seq.Append(Instance.cam.transform.DOMove(Instance.camOriginPos, 1f));
        seq.Join(Instance.cam.transform.DORotateQuaternion(Instance.camOriginRot, 1f));
        seq.SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        yield break;
    }
}
