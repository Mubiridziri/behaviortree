using UnityEngine;
public abstract class Node
{
    public enum NodeStatus { Success,  Failure, Running }

    public abstract NodeStatus Evalute();
}
