using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleCameraController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Vector3 camOriginPos;
    private Quaternion camOriginRot;
    private static BattleCameraController instance;
    private void Awake()
    {
        instance = this;
        camOriginPos = cam.transform.position;
        camOriginRot = cam.transform.rotation;
    }
    [ContextMenu("Focus")]
    public static IEnumerator FocusOnEnemy()
    {
        Vector3 camPos = NewFieldManager.Instance.enemyCard.transform.position;
        camPos += new Vector3(0f,5f,-5f);
        instance.cam.transform.DOMove(camPos,1.5f).SetEase(Ease.InSine);
        instance.cam.transform.DORotate((NewFieldManager.Instance.enemyCard.LinkedModel.transform.position - camPos).normalized, 1.5f).SetEase(Ease.InSine);
        yield return new WaitForSeconds(1.5f);
        yield break;
    }
    [ContextMenu("OutFocus")]
    public static IEnumerator OutFocusFromEnemy()
    {
        instance.cam.transform.DOMove(instance.camOriginPos, 1f).SetEase(Ease.Linear);
        instance.cam.transform.DORotateQuaternion(instance.camOriginRot, 1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        yield break;
    }
}
