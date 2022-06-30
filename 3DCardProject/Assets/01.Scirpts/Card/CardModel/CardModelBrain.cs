using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

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
                // 이미 모델오브젝트가 있는데 Set 하려 할때
                // 기존의 모델을 지우기
                Destroy(modelObject);
            }
            Debug.Log("모델 생성");
            modelObject = Instantiate(value, modelPosTrm.position, Utils.QI); ;
            anim = modelObject.GetComponent<Animator>();
           
        }
    }
    public void Move(Vector3 pos,Action act = null)
    {
        //DOTween.Kill(modelObject.transform);

        modelObject.transform.DOMove(pos, .5f).OnComplete(()=> {
            act?.Invoke();
        });
    }
}
