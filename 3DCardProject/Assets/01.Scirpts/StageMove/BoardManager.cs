using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private Transform[] childObjs;
    public List<Transform> childNodeList = new List<Transform>();

    [SerializeField] private List<GameObject> boardCams;
    [SerializeField] private TotemMove totem;

    #region
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        FillNodes();

        for(int i = 0; i < childNodeList.Count; i++)
        {
            Vector3 curPos = childNodeList[i].position;
            if(i > 0)
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
            
        foreach(Transform child in childObjs)
        {
            if(child != this.transform)
            {
                childNodeList.Add(child);
            }
        }
    }
    #endregion


    public GameObject ChangeCam(int num)
    {

        for(int i = 0; i < boardCams.Count; i++)
        {
            boardCams[i].SetActive(false);
        }
        boardCams[num].SetActive(true);
        return boardCams[num].gameObject;
    }
}
