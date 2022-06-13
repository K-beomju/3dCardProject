using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLinkedList<T>
{
    public class Node
    {
        public T Data;
        public Node PrevNode;
        public Node NextNode;
    }

    public Node head,tail;

    private List<Node> nodes = new List<Node>();

    public MyLinkedList(List<T> fields)
    {
        head = new Node();
        tail = new Node();

        head.Data = fields[0];
        tail.Data = fields[fields.Count-1];
        
        head.NextNode = tail;
        tail.PrevNode = head;
        nodes.Add(head);
        for (int i = 1; i < fields.Count -1; i++)
        {
            Node node = new Node();
            node.Data = fields[i];
            node.NextNode = head.NextNode;
            head.NextNode = node;
            node.PrevNode = node.NextNode.PrevNode;
            node.NextNode.PrevNode = node;
            nodes.Add(node);
        }

        tail.NextNode = head;
        head.PrevNode = tail;
        nodes.Add(tail);
    }

    public Node GetNodeByIndex(int idx)
    {
        return nodes[idx];
    }
    public int GetNodeCount()
    {
        return nodes.Count;
    }
    public bool isForward = true;


}
