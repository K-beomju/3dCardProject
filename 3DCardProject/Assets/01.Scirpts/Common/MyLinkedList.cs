using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLinkedList<T>
{
    public class Node
    {
        public T Data;
        public Node LastNode;
        public Node NextNode;
    }

    public Node head,tail;

    public MyLinkedList(List<T> fields)
    {
        head = new Node();
        tail = new Node();

        head.Data = fields[0];
        tail.Data = fields[fields.Count-1];
        head.NextNode = tail;
        head.LastNode = tail;

        tail.NextNode = head;
        tail.LastNode = head;

        for (int i = 1; i < fields.Count -1; i++)
        {
            Node node = new Node();
            node.Data = fields[i];
            node.LastNode = head;
            node.NextNode = tail;
            head.NextNode = node;
            tail.LastNode = node;
        }
    }

    public bool isForward = true;


}
