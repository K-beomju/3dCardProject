using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using DG.Tweening;


public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject battleField;
    [SerializeField] private GameObject shopField;
    [SerializeField] private GameObject restField;

    [SerializeField] private TotemMove totem;

    public List<Board> boardList = new List<Board>();
    //public List<GenericClass<Board>> boardArrayList = new List<GenericClass<Board>>();
    public List<Transform> childNodeList = new List<Transform>();

    private Transform[] childObjs;
    private bool isChange = false;

    [SerializeField] private GameObject totemCam;
    private Vector3 totemRotateRouteVector;
    public int[] camChangeRouteValue;


    private void Start()
    {
        FillNodes();

        for (int i = 0; i < childNodeList.Count; i++)
        {
            if (i % 2 == 0)
                boardList.Add(childNodeList[i].GetComponent<Board>());
        }

        for (int i = 0; i < boardList.Count; i++)
        {
            Board board = boardList[i];
            switch (boardList[i].type)
            {
                case StageType.Battle:
                    Instantiate(battleField, board.transform.position, Utils.QI);
                    break;
                case StageType.Shop:
                    Instantiate(shopField, board.transform.position, Utils.QI);
                    break;
                case StageType.GetHP:
                    Instantiate(restField, board.transform.position, Utils.QI);
                    break;
                case StageType.GetGold:
                    Instantiate(restField, board.transform.position, Utils.QI);
                    break;
                case StageType.LossGold:
                    Instantiate(restField, board.transform.position, Utils.QI);
                    break;
                case StageType.LossHp:
                    Instantiate(restField, board.transform.position, Utils.QI);
                    break;
                default:
                    break;
            }
        }

        //for (int i = 0; i < boardList.Count / 9; i++)
        //{
        //    boardArrayList.Add(new GenericClass<Board>());
        //    boardArrayList[i].list = new List<Board>();
        //    int shopRand = default;
        //    int restRand = default;
        //    while(shopRand == restRand)
        //    {
        //        shopRand = UnityEngine.Random.Range(0, 10);
        //        restRand = UnityEngine.Random.Range(0, 10);
        //    }
        //    print(shopRand + " " + restRand);

        //    for (int k = 0; k < 9; k++)
        //    {
        //        Board board = boardList[i * 9 + k];
        //        if (i != 0 || k != 0)
        //        {
        //            if (k == shopRand)
        //            {
        //                board.type = StageType.Shop;
        //                GameObject shopFd = Instantiate(shopField, board.transform.position, Utils.QI);
        //            }
        //            else if(k == restRand)
        //            {
        //                board.type = StageType.Rest;
        //                GameObject shopFd = Instantiate(restField, board.transform.position, Utils.QI);
        //            }
        //            else
        //            {
        //                board.type = StageType.Battle;
        //                board.enemyType = EnemyType.MEDIUM;
        //                GameObject battleFd = Instantiate(battleField, board.transform.position, Utils.QI);
        //            }
        //        }
        //        boardArrayList[i].list.Add(board);
        //    }
        //}

    }

    #region
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        FillNodes();
        for (int i = 0; i < childNodeList.Count; i++)
        {
            Vector3 curPos = childNodeList[i].position;
            if (i > 0)
            {
                Vector3 prevPos = childNodeList[i - 1].position;
                Gizmos.DrawLine(prevPos, curPos);
            }
        }
    }

    public void FillNodes()
    {
        childNodeList.Clear();
        childObjs = GetComponentsInChildren<Transform>();

        foreach (Transform child in childObjs)
        {
            if (child != this.transform)
            {
                childNodeList.Add(child);

            }
        }
    }

    public void CheckRouteCam(bool soft)
    {
        if (totemCam == null) return;

        for (int i = 0; i < camChangeRouteValue.Length; i++)
        {
            if (totem.routePosition >= camChangeRouteValue[i])
            {
                switch (i)
                {
                    case 0:
                        totemRotateRouteVector = new Vector3(totemCam.transform.eulerAngles.x, -93, totemCam.transform.eulerAngles.z);
                        break;
                    default:
                        break;
                }

                if (soft)
                    totemCam.transform.DORotate(totemRotateRouteVector, 1);
                else
                    totemCam.transform.rotation = Quaternion.Euler(totemRotateRouteVector);

            }
        }
    }


    #endregion



}
