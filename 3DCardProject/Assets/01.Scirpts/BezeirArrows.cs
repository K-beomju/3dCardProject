using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BezeirArrows : MonoBehaviour
{
    public GameObject arrowHead;
    public GameObject arrowBody;
    public int arrowBodyNum;
    public float scaleFactor = 1f;

    private RectTransform origin;
    private List<RectTransform> arrowBodys = new List<RectTransform>();
    private List<Vector2> controlPoints = new List<Vector2>();
    private readonly List<Vector2> controlPointFactors = new List<Vector2>() { new Vector2(0f, 0.6f), new Vector2(0.1f, 1.2f) };

    private void Awake()
    {
        this.origin = this.GetComponent<RectTransform>();

        for (int i = 0; i < this.arrowBodyNum; i++)
        {
            this.arrowBodys.Add(Instantiate(this.arrowBody, this.transform).GetComponent<RectTransform>());

        }
        this.arrowBodys.Add(Instantiate(this.arrowHead, this.transform).GetComponent<RectTransform>());

        this.arrowBodys.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));

        for (int i = 0; i < 4; ++i)
        {
            this.controlPoints.Add(Vector2.zero);
        }
    }

    private void Update()
    {
        this.controlPoints[0] = new Vector2(this.origin.position.x, this.origin.position.y - 350);

        this.controlPoints[3] = new Vector2(Input.mousePosition.x, Input.mousePosition.y);


        this.controlPoints[1] = this.controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[0];
        this.controlPoints[2] = this.controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[1];


        for (int i = 0; i < arrowBodys.Count; ++i)
        {
            var t = Mathf.Log(1f * i / (arrowBodys.Count - 1) + 1f, 2f);

            arrowBodys[i].position =
                Mathf.Pow(1 - t, 3) * controlPoints[0] +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1] +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2] +
                Mathf.Pow(t, 3) * controlPoints[3];

            if (i > 0)
            {
                var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, arrowBodys[i].position - arrowBodys[i - 1].position));
                arrowBodys[i].rotation = Quaternion.Euler(euler);
            }

            var scale = scaleFactor * (1f - 0.03f * (arrowBodys.Count - 1 - i));
            arrowBodys[i].localScale = new Vector3(scale, scale, 1f);

        }

        arrowBodys[0].transform.rotation = arrowBodys[1].transform.rotation;


    }

    public void ActiveArrow(bool isActive)
    {
        gameObject.SetActive(isActive);
        Cursor.visible = !isActive;

    }
}
