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

    public Node head;

    private List<Node> nodes = new List<Node>();

    public MyLinkedList(List<T> fields)
    {
        head = new Node();

        Init_Head(fields[0]);
        
        nodes.Add(head);
        for (int i = 1; i < fields.Count ; i++)
        {
            Insert_Head(fields[i]);
            /*Node node = new Node();
                node.Data = fields[i];
                node.NextNode = head.NextNode;
                head.NextNode = node;
                node.PrevNode = node.NextNode.PrevNode;
                node.NextNode.PrevNode = node;*/
        }

    }
    private void Init_Head(T data)
    {
        head = CreateNode(data);

        if (head == null) return;

        head.Data = data;
        head.PrevNode = head.NextNode = head;
    }
    Node CreateNode(T data)
    {
        Node New_Node = new Node();

        if (New_Node == null) return New_Node;

        New_Node.Data = data;
        New_Node.PrevNode= New_Node.NextNode = null;

        return New_Node;
    }

    void Insert_Head(T data)
    {
        Node New_Node = CreateNode(data);
        nodes.Add(New_Node);

        if (New_Node == null) return;

        // 새로운 노드 연결.
        New_Node.PrevNode = head;
        New_Node.NextNode= head.NextNode;
        New_Node.PrevNode.NextNode= New_Node;
        New_Node.NextNode.PrevNode = New_Node;

        // data 삽입.
        New_Node.Data = data;
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
