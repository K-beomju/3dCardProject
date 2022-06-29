using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardModelBrain : MonoBehaviour
{
    [SerializeField]
    private Transform modelPosTrm;

    private Animator anim;

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
           
        }
    }
    public void Move(Vector3 pos)
    {
        //DOTween.Kill(transform);

        modelObject.transform.DOMove(pos, .5f);
    }
}
