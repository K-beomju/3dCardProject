using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private ParticleSystem pt;
    [SerializeField] private ParticleSystem pt2;

    [SerializeField] private ParticleSystem pt3;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        pt.gameObject.SetActive(true);
        pt.Play();
        pt.transform.position = transform.position + new Vector3(0,0.3f,0);
        pt2.gameObject.SetActive(true);
        pt2.Play();
        pt2.transform.position = transform.position + new Vector3(0, 0.3f, 0);
    }

    public void MegaPt(Transform pos)
    {
        pt3.gameObject.SetActive(true);
        pt3.Play();
        pt3.transform.position = pos.position;
    }
}
