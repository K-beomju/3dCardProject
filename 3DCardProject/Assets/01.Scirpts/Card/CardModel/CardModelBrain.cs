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
            if(modelObject != null)
            {
                // �̹� �𵨿�����Ʈ�� �ִµ� Set �Ϸ� �Ҷ�
                // ������ ���� �����
                Destroy(modelObject);
            }
            Debug.Log("�� ����");
            modelObject = Instantiate(value, modelPosTrm.position, Utils.QI); ;
            anim = modelObject.GetComponent<Animator>();
            modelObject.transform.DORotate(new Vector3(0,-180,0), .2f);

        }
    }
    public void Move(Vector3 pos,Action act = null)
    {
        //DOTween.Kill(modelObject.transform);
        Vector3 des = new Vector3(pos.x, modelObject.transform.position.y, pos.z);
        Vector3 dir = (pos - modelObject.transform.position).normalized;
        anim?.SetBool("isMove", true);
        
        modelObject.transform.DORotate(Quaternion.LookRotation(dir).eulerAngles,.1f);
        modelObject.transform.DOMove(pos, .6f).OnComplete(() => {
            modelObject.transform.DORotate(new Vector3(0,-180,0), 1);
            anim?.SetBool("isMove", false);
            act?.Invoke();
        });
    }

    public void JumpMove(Vector3 pos, Action act = null)
    {
        modelObject.transform.DOJump(pos,3, 0 ,.5f, false).OnComplete(() => {
            act?.Invoke();
            NewFieldManager.Instance.isFrontJumping = false;
        });
    }
}
