using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using DG.Tweening;


public class BoardManager : MonoBehaviour
{
    public List<Board> boardList = new List<Board>();
    public List<GenericClass<Board>> boardArrayList = new List<GenericClass<Board>>();
    public List<Transform> childNodeList = new List<Transform>();

    private Transform[] childObjs;
    private bool isChange = false;

    [SerializeField] private GameObject battleField;
    [SerializeField] private GameObject shopField;
    [SerializeField] private TotemMove totem;



    //public List<GameObject> stoneList = new List<GameObject>();

    private void Start()
    {
        FillNodes();

        for (int i = 0; i < childNodeList.Count; i++)
        {
            boardList.Add(childNodeList[i].GetComponent<Board>());
        }
        for (int i = 0; i < childNodeList.Count / 9; i++)
        {
            boardArrayList.Add(new GenericClass<Board>());
            boardArrayList[i].list = new List<Board>();
            int rand = UnityEngine.Random.Range(0, 10);

            for (int k = 0; k < 9; k++)
            {
                Board board = boardList[i * 9 + k];

                if (i != 0 || k != 0)
                {
                   

                    if (k == rand)
                    {
                        board.type = StageType.Shop;
                        GameObject shopFd = Instantiate(shopField, board.transform.position , Utils.QI);
                        //stoneList.Add(shopFd);
                    }
                    else
                    {
                        GameObject battleFd = Instantiate(battleField, board.transform.position - new Vector3(0,0.1f,0) , Utils.QI);
                        //stoneList.Add(battleFd);

                    }
                }


                boardArrayList[i].list.Add(board);

            }
        }




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
    #endregion

    private bool isClear = false;
    int num = 0;
    public int[] camChangeValues;

   
    public void ClearBoard(int routePos)
    {
        boardList[routePos + 1].ClearAction();
    }
}
