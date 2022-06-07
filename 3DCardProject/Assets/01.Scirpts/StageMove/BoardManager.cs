using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private Transform[] childObjs;
    public List<Transform> childNodeList = new List<Transform>();

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
}
