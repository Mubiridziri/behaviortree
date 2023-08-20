using System.Collections;
using UnityEngine;


public class InverterNode : Node
{
    private Node node;

    public InverterNode(Node node)
    {
        this.node = node;
    }

    public override NodeStatus Evalute()
    {
        NodeStatus status = node.Evalute();


        if (status == NodeStatus.Success)
        {
            return NodeStatus.Failure;
        }

        return NodeStatus.Success;
    }
}
