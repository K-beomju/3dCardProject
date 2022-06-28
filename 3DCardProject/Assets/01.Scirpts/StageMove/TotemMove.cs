using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TotemMove : MonoBehaviour
{
    public BoardManager board;
    public int steps;
    

    public int routePosition;
    private bool isMove = false;
    public float speed;


    #region Dice
    public GameObject dice;
    private Text diceText;
    public Camera cam;

    private bool isRoll = false;
    private WaitForSeconds rollDelay = new WaitForSeconds(0.1f);
    private IEnumerator rollCo;
    #endregion

    private void Awake()
    {
        diceText = dice.GetComponent<Text>();
        rollCo = RollingDice();
        dice.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isRoll)
            {
                dice.SetActive(true);
                StartCoroutine(rollCo);
                isRoll = true;
            }
            else
            {
                StopCoroutine(rollCo);
                isRoll = false;


                if (!isMove)
                {
                    if (routePosition + steps < board.childNodeList.Count)
                    {
                        StartCoroutine(MoveMentCo());
                    }
                }
            }
        }

        if(isMove)
            dice.transform.position = cam.WorldToScreenPoint(transform.position + new Vector3(0, 0.8f, 0));


    }

    private IEnumerator MoveMentCo()
    {
        yield return new WaitForSeconds(0.5f);

        if (isMove) yield break;
        isMove = true;
        yield return new WaitForSeconds(1f);

        while(steps > 0)
        {
            Vector3 nextPos = board.childNodeList[routePosition + 1].transform.position;
            while(MoveNextNode(nextPos))
                yield return null;  

            // 보드 클리어 검사 
            board.ClearBoard(routePosition);
            yield return new WaitForSeconds(0.1f);
            steps--;
            diceText.text = steps.ToString();
            routePosition++;
            board.ChangeCam();
        }

        yield return new WaitForSeconds(0.5f);
        isMove = false;
        dice.SetActive(false);
    }

    private IEnumerator RollingDice()
    {
        while (true)
        {
            steps = Random.Range(1, 7);
            diceText.text = steps.ToString();
            yield return rollDelay; 
        }
    }
   

    private bool MoveNextNode(Vector3 goal)
    {
        return goal != (transform.position  = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime));
    }

   

}
