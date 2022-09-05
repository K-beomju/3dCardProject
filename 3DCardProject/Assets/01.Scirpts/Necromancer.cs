using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        
        anim = GetComponent<Animator>();
    }
}
