using System;

public class ActionNode : Node
{
    private Func<NodeStatus> action;

    public ActionNode(Func<NodeStatus> action)
    {
        this.action = action;
    }

    public override NodeStatus Evalute()
    {
        return action();
    }
}
