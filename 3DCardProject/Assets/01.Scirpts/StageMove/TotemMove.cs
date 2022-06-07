using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TotemMove : MonoBehaviour
{
    public BoardManager board;
    public int steps;
    

    public int routePosition;
    private bool isMove = false;
    public float speed;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isMove)
        {
            steps = Random.Range(1, 7);

            if(routePosition + steps < board.childNodeList.Count)
            {
                StartCoroutine(MoveMentCo());
            }
        }
    }

    private IEnumerator MoveMentCo()
    {
        if(isMove) yield break;
        isMove = true;

        while(steps > 0)
        {
            Vector3 nextPos = board.childNodeList[routePosition + 1].position;
            while(MoveNextNode(nextPos)) 
                yield return null;  

            yield return new WaitForSeconds(0.1f);
            steps--;
            routePosition++;

            CheckNode();
        }

        isMove = false;
    }

    private bool MoveNextNode(Vector3 goal)
    {
        return goal != (transform.position  = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

    public void CheckNode()
    {
        switch(routePosition)
        {
            case 8:
                board.ChangeCam(1);
                break;
        }
    }

}
