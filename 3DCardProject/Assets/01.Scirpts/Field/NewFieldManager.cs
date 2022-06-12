using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFieldManager : Singleton<NewFieldManager>
{
    private MyLinkedList<Field> fields;

    public List<Field> fieldList = new List<Field>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        fields = new MyLinkedList<Field>(fieldList);

    }
}
