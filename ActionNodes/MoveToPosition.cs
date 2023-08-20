using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MoveToPosition : Node
{
    private NavMeshAgent agent;
    private Vector3 targetPosition;

    public MoveToPosition(NavMeshAgent agent, Vector3 startingPoint)
    {
        this.agent = agent;
        this.targetPosition = startingPoint;
    }

    public override NodeStatus Evalute()
    {
        Vector3 currentPosition = agent.gameObject.transform.position;

        if(Vector3.Distance(currentPosition, targetPosition) < 0.1f)
        {
            agent.ResetPath();
            return NodeStatus.Success;
        }
        agent.SetDestination(targetPosition);
        return NodeStatus.Running;
    }
}
