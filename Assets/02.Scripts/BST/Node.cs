using UnityEngine;

public class Node
{
    public int value;

    public Node left;
    public Node right;

    public Vector3 position;
    public GameObject visualObject;

    public Node(int value)
    {
        this.value = value;
        left = null;
        right = null;

        position = Vector3.zero;
        visualObject = null;
    }    
}
