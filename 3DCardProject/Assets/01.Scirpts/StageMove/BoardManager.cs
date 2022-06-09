using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using DG.Tweening;


public class BoardManager : MonoBehaviour
{
    public List<Board> boardList = new List<Board>();

    private Transform[] childObjs;
    public List<Transform> childNodeList = new List<Transform>();

    [SerializeField] private List<CinemachineVirtualCamera> boardCamList = new List<CinemachineVirtualCamera>();
    [SerializeField] private List<Board> boardCamChList = new List<Board>();

    public CanvasGroup fadePanel;

    public List<GenericClass<Board>> boardArrayList = new List<GenericClass<Board>>(); 



    private bool isChange = false;

    private void Start()
    {
        FillNodes();

        for (int i = 0; i < childNodeList.Count; i++)
        {
            boardList.Add(childNodeList[i].GetComponent<Board>());
        }
        for (int i = 0; i < childNodeList.Count / 6; i++)
        {
            boardArrayList.Add(new GenericClass<Board>());
            boardArrayList[i].list = new List<Board>();
            int rand = UnityEngine.Random.Range(0, 7);

            for (int k = 0; k < 6; k++)
            {
                Board board = boardList[i * 6 + k];
                if(k == rand)
                {
                    board.type = StageType.Shop;
                }
                board.stageData = StageManager.Instance.stageArray[i];
                board.avatar = StageManager.Instance.stageArray[i].avatar.item.ShallowCopy();
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
    public void ChangeCam()
    {
        for (int i = num; i < boardCamList.Count; i++)
        {
            boardCamList[i].gameObject.SetActive(false);
            if (boardCamChList[i].isClear && !isClear)
            {
                num++;
                isClear = true;
            }
        }

        if (isClear)
        {
            boardCamList[num - 1].gameObject.SetActive(true);
            isClear = false;
        }
    }

    public void ClearBoard(int routePos)
    {
        boardList[routePos + 1].ClearAction();
    }
}
