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
                // �̹� �𵨿�����Ʈ�� �ִµ� Set �Ϸ� �Ҷ�
                // ������ ���� �����
                Destroy(modelObject);
            }
            Debug.Log("�� ����");
            modelObject = Instantiate(value, modelPosTrm.position, Utils.QI); ;
            anim = modelObject.GetComponent<Animator>();
            //modelObject.transform.DORotate(new Vector3(0, yValue, 0), .2f);

        }
    }
    public void Move(Vector3 pos, Action act = null,Action subAct = null)
    {
        //DOTween.Kill(modelObject.transform);
        Vector3 des = new Vector3(pos.x, modelObject.transform.position.y, pos.z);
        Vector3 dir = (pos - modelObject.transform.position).normalized;
        anim?.SetBool("isMove", true);

        modelObject.transform.DORotate(Quaternion.LookRotation(dir).eulerAngles, .1f);
        modelObject.transform.DOMove(pos, .6f).OnComplete(() =>
        {

            //modelObject.transform.DORotate(new Vector3(0,-180,0), 1);
            if (TurnManager.Instance.Type == TurnType.Player)
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
            subAct?.Invoke();
        });
    }




    public void JumpMove(Vector3 pos,Vector3 dir, Action act = null,Action subAct = null)
    {
        anim?.SetBool("isMove", true);
        Vector3 modelDir = (pos - modelObject.transform.position).normalized;
        print(pos);
        print(modelDir);
        print(dir);
        /*  if (TurnManager.Instance.Type == TurnType.Player)
          {

          }
          else
          {
              Move(pos, act);
          }*/
        Sequence seq = DOTween.Sequence();
        seq.Append(modelObject.transform.DOMove(pos - dir * 3, .6f));
        seq.Join(modelObject.transform.DORotate(Quaternion.LookRotation(modelDir).eulerAngles, .1f));
        seq.Append(modelObject.transform.DOJump(pos + dir * 3, 3, 0, .5f, false));
        seq.Join(modelObject.transform.DORotate(Quaternion.LookRotation(dir).eulerAngles, .1f));
        seq.OnComplete(() =>
        {
            //�߸ŷ� �� �ѱ�� �ð� ����
            subAct?.Invoke();

            anim?.SetBool("isMove", false);
            //NewFieldManager.Instance.isFrontJumping = false;
        });
    }
}
