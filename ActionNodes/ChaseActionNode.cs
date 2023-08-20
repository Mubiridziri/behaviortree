using UnityEngine;
using UnityEngine.AI;

public class ChaseActionNode : Node
{
    private GameObject target;
    private NavMeshAgent agent;

    public ChaseActionNode(GameObject target, NavMeshAgent agent)
    {
        this.target = target;
        this.agent = agent;
    }

    public override NodeStatus Evalute()
    {
        if (Vector3.Distance(agent.gameObject.transform.position, target.transform.position) < 1f)
        {
            agent.ResetPath();
            return Node.NodeStatus.Success;
        }
        agent.SetDestination(target.transform.position);

        return Node.NodeStatus.Running;
    }
}
