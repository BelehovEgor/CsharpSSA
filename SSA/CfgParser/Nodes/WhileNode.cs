using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public class WhileNode : Node
{
    public PossibleValue Condition { get; init; }
    public Node? True { get; init; }

    public WhileNode(PossibleValue condition, Node? trueNode)
    {
        Condition = condition;
        True = trueNode;
        
        if (True is not null) AddMember(True);
    }
    
    public override void AddNext(Node node)
    {
        AddMember(node);

        MakeLoop(True);
    }

    public override ICollection<Variable> GetNodeVariables()
    {
        return Condition.GetAllVariables();
    }

    public override string ToString()
    {
        return Condition.Value.ToString() ?? true.ToString();
    }

    private void MakeLoop(Node? node)
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