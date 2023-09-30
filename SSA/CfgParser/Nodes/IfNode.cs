using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public sealed class IfNode : Node
{
    public PossibleValue Condition { get; init; }
    public Node? True { get; init; }
    public Node? False { get; init;  }

    public IfNode(PossibleValue condition, Node? trueNode, Node? falseNode)
    {
        Condition = condition;
        True = trueNode;
        False = falseNode;

        if (True is not null)
        {
            AddMember(True);
        }
        if (False is not null) AddMember(False);
    }
    
    public override void AddNext(Node node)
    {
        if (True is null || False is null) AddMember(node);

        AddNextInternal(True, node);
        AddNextInternal(False, node);
    }

    public override ICollection<Variable> GetNodeVariables()
    {
        return Condition.GetAllVariables();
    }

    public override string ToString()
    {
        return Condition.Value.ToString() ?? string.Empty;
    }

    private void AddNextInternal(Node? node, Node next)
    {
        var trueLastNodes = node?.GetLastReturnsNodesFromBlock();
        if (trueLastNodes is not null)
        {
            foreach (var lastNode in trueLastNodes)
            {
                lastNode.AddNext(next);
            }
        }
    }
}