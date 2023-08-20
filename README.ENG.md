# Behavior Tree for Unity 3D
>ENG: A simple example of implementing behavior tree for opponents in your game.
____

## What is Behavior Tree?

Behavior tree — a mathematical model of plan execution used in computer science, robotics, control systems and video games. They describe switchings between a finite set of tasks in a modular fashion. Their strength comes from their ability to create very complex tasks composed of simple tasks, without worrying how the simple tasks are implemented. Behavior trees present some similarities to hierarchical state machines with the key difference that the main building block of a behavior is a task rather than a state. Its ease of human understanding make behavior trees less error prone and very popular in the game developer community. Behavior trees have been shown to generalize several other control architectures.

#### In simple words, this means

This is a set of actions and conditions before these actions combined into some tree-like format that determines which of the actions to be performed. An example will be below.

We have some object that will perform actions:

  - Chase the player if he sees him
  - Return to the starting point if you lost a player

We can distinguish two branches of behavior of the same name from this: pursue and go back

Chase

What should be checked before pursuing? Well, probably what we see is the object of persecution. If not, then this branch of behavior does not make sense.

Return to starting point

What should be checked before returning? Well, probably that we don't see the object anymore, otherwise it would be strange that we went to the starting point.

It turns out that our behavior tree should look something like this:

![Tree Example Image](tree-example.png "Tree Example")

## What is what?

A node ([Node](Builder/Node.cs)) is any object in a behavior (decision) tree. Each node has a state, in my case [NodeStatus](Builder/Node.cs) (enum), which in turn has the following values:

  - NodeStatus.Success
  - NodeStatus.Failure
  - NodeStatus.Running

rootNode - this is the main node from which the whole tree exits.

Selector ([Selector](Builder/SelectorNode.cs)) is a type of node that "selects" one of its child nodes. That is, it waits for NodeStatus.Success or NodeStatus.Running from at least one of its child nodes.


Sequence ([Sequence](Builder/SequenceNode.cs)) is the type of node that executes all child nodes. That is, it is important for him that __all child elements__ return NodeStatus.Success.


A decorator is a kind of node that is not yet explicitly represented in these sources, but is an important element of the behavior tree. This node conducts checks and has a link to the next task (node), and if successful, they execute it. In these source codes, I cheated a little. I use ordinary nodes for checks, and in order for the next task to be executed only if successful, I wrap them in a Sequence.

## How to use this tree?

Well, for starters, you should copy the Builder, ActionNodes, DecoratorNodes into your project.

Create a standard C# class using Unity 3D. First of all, we need to create a property in which we will store our tree.

```C#
public class EnemyAI : MonoBehavior
{
     private Node rootNode;
```

Next, we will describe the behavior tree that we considered in the examples above. Let's create a function that will build our business.

```C#
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
```
We see how our two branches are created. I want to draw your attention to the fact that I suggested two ways to describe an ActionNode, that is, a node that performs some kind of action.

The first is through [ActionNode](Builder/ActionNode.cs), that is, a pre-prepared way, where the input argument is a function with a return type of Node.NodeStatus. This is convenient if you want to have functions in this class and you want to use them, or if your entire tree will be in one script. However, I don't recommend this method.

The second way is to describe an independent node, such as [ChaseActionNode](ActionNodes/ChaseActionNode.cs).

Both isTargetSees and chaseAction are nodes by type, however, one is created through the transfer of a function, and the second is described by an independent class.

Each branch, as I said, I cheated and wrapped in Sequence. Then I passed them to the Selector, which will be our rootNode.

We need to build our tree and assign a value to the property:

```C#
     public void Start()
     {
         buildBehaviorTree();
     }
```

It remains to run our tree, which must be executed every frame (frame). This is already easy to do:

```C#
     // Update is called once per frame
     public void Update()
     {
         rootNode.Evaluate();
     }
```

That's it, good luck!