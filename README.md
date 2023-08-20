# Behavior Tree for Unity 3D
> Простой пример реализации дерева поведения (решений) противников в вашей игре.

This description in English can be read [here](/README.ENG.md).
____

## What is Behavior Tree? / Что такое дерево поведения (решений)?

Дерево поведения — это математическая модель выполнения плана , используемая в информатике , робототехнике , системах управления и видеоиграх . Они описывают переключение между конечным набором задач по модульному принципу. Их сила заключается в их способности создавать очень сложные задачи, состоящие из простых задач, не беспокоясь о том, как эти простые задачи реализуются. Деревья поведения имеют некоторое сходство с иерархическими конечными автоматами.с той ключевой разницей, что основным строительным блоком поведения является задача, а не состояние. 

#### Простыми словами

Это набор действий и условий перед этими действиями объединеных в некоторый древовидный формат, который определяет какое из действий нужно выполнить. Пример будет ниже.

У нас есть некоторый объект, который будет выполнять действия:

 - Преследовать игрока, если его видит
 - Возвращаться на стартовую точку, если потерял игрока

Мы можем выделить из этого одноименные две ветки поведения:  преследовать и идти назад

Преследовать

Что нужно проверить перед тем, как преследовать? Ну, наверное то, что мы видим объект преследования. Если нет, то эту ветку поведения выполнять не имеет смысла.

Возвращаться на стартовую точку

Что нужно проверить перед тем, как возвращаться? Ну, наверное то, что мы больше не видим объект, иначе будет странно, что мы пошли на стартовую точку.

Получается, что наше дерево поведения должно выглядеть примерно так:

![Tree Example Image](tree-example.png "Tree Example")


## Что есть что?

Нода ([Node](Builder/Node.cs)) — это любой объект в дереве поведения (решений). Каждая нода имет состояние, в моём случае [NodeStatus](Builder/Node.cs) (enum), который в свою очередь имеет следующие значения:

 - NodeStatus.Success
 - NodeStatus.Failure
 - NodeStatus.Running

rootNode - это основная нода из которой выходим всё дерево.

Selector ([Selector](Builder/SelectorNode.cs)) — такой тип ноды, который "выбирает" одну из своих дочерних нод. То есть, он ждет NodeStatus.Success или NodeStatus.Running хотя бы от одной из своих дочерних нод.


Sequence ([Sequence](Builder/SequenceNode.cs)) — тип ноды, который выполняет все дочерние ноды. То есть, ему важно, чтобы __всё дочение элементы__ вернули NodeStatus.Success.


Decorator — такой вид ноды, который пока явно не представлен в этих исходных кодах, но является важным элементом дерева поведения. Данная нода проводит проверки и имеют ссылку на следующую задачу (ноду), и в случае успеха выполняют её. В данных исходных кодах я немного схитрил. Я использую для проверок обычные ноды, а для того, чтобы следующая задача выполнялась только в случае успеха оборачиваю их в Sequence.

## Как пользоваться этим деревом?

Ну для начала вам стоит скопировать Builder, ActionNodes, DecoratorNodes в ваш проект.

Создаем стандартный C# класс через Unity 3D. Первым делом, нам нужно завести свойство, в котором мы будем хранить наше дерево. 

```C#
public class EnemyAI : MonoBehaviour
{
    private Node rootNode;
```

Дальше мы будем описывать дерево поведения, которое мы рассматривали в примерах выше. Создадим функцию, которая будет строить наше делево.

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
Мы видим как создаются две наших ветки. Хочу обратить внимание, что я предложил два способа описания ActionNode, то есть ноды, которая выполняет какое-то действие.  

Первый, это через [ActionNode](Builder/ActionNode.cs), то есть заранее подготовленный способ, где входным аргументом является функция с возвращаемым типом Node.NodeStatus. Это удобно, если хотите есть функции в этом классе и вы хотите их использовать, либо же всё ваше дерево будет в одном скрипте. Хотя, я вам такой способ не советую.

Второй способ, это описание самостоятельной ноды, такой как [ChaseActionNode](ActionNodes/ChaseActionNode.cs). 

Что isTargetSees, что chaseAction, являются по типу нодами, однако одна создача через передачу функции, а вторая описана самостоятельным классом.

Каждую ветку, как я и говорил, я схитрил и обернул в Sequence. Затем передал их в Selector, который будет является нашим rootNode.

Нужно построить наше дерево и присвоить значение свойству:

```C#
    public void Start()
    {
        buildBehaviorTree();
    }
```

Остается запускать наше дерево, котоое должно выполняеться каждый фрейм (кадр). Это уже сделать легко:

```C#
    // Update is called once per frame
    public void Update()
    {
        rootNode.Evalute();
    }
```

Вот и всё, удачи!