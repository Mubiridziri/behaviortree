using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    private Node rootNode;
    private bool isSeesPlayer;
    private Vector3 startingPoint;

    // Use this for initialization
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        startingPoint = transform.position;
        buildBehaviorTree();
    }

    private void buildBehaviorTree()
    {
        Node isTargetSees = new ActionNode(IsTargerSees);
        Node chaseAction = new ChaseActionNode(player, agent);
        Node chaseSequence = new SequenceNode(new List<Node>() { isTargetSees, chaseAction });


        Node isNotTargetSees = new InverterNode(new ActionNode(IsTargerSees));
        Node moveToStartAction = new MoveToPosition(agent, startingPoint);
        Node moveToStartSequence = new SequenceNode(new List<Node>() { isNotTargetSees, moveToStartAction });

        rootNode = new SelectorNode(new List<Node>() { chaseSequence, moveToNoiseSequence, moveToStartSequence });
    }

    // Update is called once per frame
    public void Update()
    {
        rootNode.Evalute();
    }


    private Node.NodeStatus IsTargerSees()
    {
        return isSeesPlayer ? Node.NodeStatus.Success : Node.NodeStatus.Failure;
    }
}
