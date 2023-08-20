using System.Collections.Generic;

public class SelectorNode : Node
{
    private readonly List<Node> children;

    public SelectorNode(List<Node> children)
    {
        this.children = children;
    }

    public override NodeStatus Evalute()
    {
        foreach (Node node in children)
        {
            NodeStatus status = node.Evalute();

            if(status != NodeStatus.Failure)
            {
                return status;
            }
        }

        return NodeStatus.Failure;
    }
}
