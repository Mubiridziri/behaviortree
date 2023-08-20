using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : Node
{
    private readonly List<Node> children;

    public SequenceNode(List<Node> children)
    {
        this.children = children;
    }

    public override NodeStatus Evalute()
    {
        foreach (Node node in children)
        {
            NodeStatus status = node.Evalute();
            if(status != NodeStatus.Success)
            {
                return status;
            }
        }

        return NodeStatus.Success;
    }
}
