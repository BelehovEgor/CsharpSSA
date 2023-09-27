using SSA.Nodes.Extensions;
using SSA.Nodes.Models;
using OneOf;

namespace SSA.Nodes;

using PossibleValue = OneOf<BinaryExpressionVariable, Variable, string?>;

public class WhileNode : NodeBase
{
    public ICollection<PossibleValue>  Condition { get; init; }
    public INode? True { get; init; }

    public WhileNode(PossibleValue condition, INode? trueNode)
        : this(new[] {condition}, trueNode)
    {
    }

    public WhileNode(ICollection<PossibleValue> condition, INode? trueNode)
    {
        Condition = condition;
        True = trueNode;
        
        if (True is not null) MembersList.Add(True);
    }
    
    public override void AddNext(params INode[] nodes)
    {
        if (nodes.Length != 1) throw new InvalidOperationException();
        
        MembersList.AddRange(nodes);

        MakeLoop(True);
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, Condition.Select(x => x.Value.ToString()));
    }

    private void MakeLoop(INode? node)
    {
        var trueLastNodes = node?.GetLastReturnsNodesFromBlock();
        if (trueLastNodes is not null)
        {
            foreach (var lastNode in trueLastNodes)
            {
                lastNode.AddNext(this);
            }
        }
    }
}