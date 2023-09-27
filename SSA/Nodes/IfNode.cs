using SSA.Nodes.Models;
using OneOf;
using SSA.Nodes.Extensions;

namespace SSA.Nodes;

using PossibleValue = OneOf<BinaryExpressionVariable, Variable, string?>;

public sealed class IfNode : NodeBase
{
    public PossibleValue Condition { get; init; }
    public INode? True { get; init; }
    public INode? False { get; init;  }

    public IfNode(PossibleValue condition, INode? trueNode, INode? falseNode)
    {
        Condition = condition;
        True = trueNode;
        False = falseNode;
        
        if (True is not null) MembersList.Add(True);
        if (False is not null) MembersList.Add(False);
    }
    
    public override void AddNext(params INode[] nodes)
    {
        if (nodes.Length != 1) throw new InvalidOperationException();
        
        if (True is null || False is null) MembersList.AddRange(nodes);

        AddNextInternal(True, nodes);
        AddNextInternal(False, nodes);
    }

    public override string ToString()
    {
        return Condition.Value.ToString() ?? string.Empty;
    }

    private void AddNextInternal(INode? node, INode[] nodes)
    {
        var trueLastNodes = node?.GetLastReturnsNodesFromBlock();
        if (trueLastNodes is not null)
        {
            foreach (var lastNode in trueLastNodes)
            {
                lastNode.AddNext(nodes);
            }
        }
    }
}