using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CardModelBrain : MonoBehaviour
{
    [SerializeField]
    private Transform modelPosTrm;

    public Animator anim { get; set; }
    private GameObject modelObject;
    public GameObject ModelObject
    {
        get
        {
            return modelObject;
        }
        set
        {
            if (modelObject != null)
            {
                // 이미 모델오브젝트가 있는데 Set 하려 할때
                // 기존의 모델을 지우기
                Destroy(modelObject);
            }
            Debug.Log("모델 생성");
            modelObject = Instantiate(value, modelPosTrm.position, Utils.QI); ;
            anim = modelObject.GetComponent<Animator>();
            //modelObject.transform.DORotate(new Vector3(0, yValue, 0), .2f);

        }
    }
    public void Move(Vector3 pos, Action act = null)
    {
        //DOTween.Kill(modelObject.transform);
        Vector3 des = new Vector3(pos.x, modelObject.transform.position.y, pos.z);
        Vector3 dir = (pos - modelObject.transform.position).normalized;
        anim?.SetBool("isMove", true);

        modelObject.transform.DORotate(Quaternion.LookRotation(dir).eulerAngles, .1f);
        modelObject.transform.DOMove(pos, .6f).OnComplete(() =>
        {

            //modelObject.transform.DORotate(new Vector3(0,-180,0), 1);
            if (TurnManager.Instance.type == TurnType.Player)
            {
                if(NewFieldManager.Instance.IsClockDir)
                modelObject.transform.DOLookAt(NewFieldManager.Instance.GetPlayerNodeByData().NextNode.Data.transform.position, .5f);
                else
                  modelObject.transform.DOLookAt(NewFieldManager.Instance.GetPlayerNodeByData().PrevNode.Data.transform.position, .5f);

            }
            else
            {
                if (NewFieldManager.Instance.IsClockDir)
                    modelObject.transform.DOLookAt(NewFieldManager.Instance.GetEnemyNodeByData().NextNode.Data.transform.position, .5f);
                else
                    modelObject.transform.DOLookAt(NewFieldManager.Instance.GetEnemyNodeByData().PrevNode.Data.transform.position, .5f);
            }

            anim?.SetBool("isMove", false);
            act?.Invoke();
        });
    }




    public void JumpMove(Vector3 pos, Action act = null)
    {
        if (TurnManager.Instance.type == TurnType.Player)
        {
            modelObject.transform.DOJump(pos, 3, 0, .5f, false).OnComplete(() =>
            {
                act?.Invoke();
                NewFieldManager.Instance.isFrontJumping = false;
            });

        }
        else
        {
            Move(pos, act);
        }
    }
}
