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

        ProcessBreaks(node);
        ProcessContinues();
    }

    protected void ProcessBreaks(Node node)
    {
        var breakNodes = this.GetBreakNodes();
        foreach (var breakNode in breakNodes)
        {
            foreach (var parent in breakNode.Parents.ToArray())
            {
                parent.SwapMember(breakNode, node);
            }
        }
    }

    protected void ProcessContinues()
    {
        var continueNodes = this.GetContinueNodes();
        foreach (var continueNode in continueNodes)
        {
            foreach (var parent in continueNode.Parents.ToArray())
            {
                parent.SwapMember(continueNode, this);
            }
        }
    }
    
    public override ICollection<Variable> GetNodeVariables()
    {
        return Condition.GetAllVariables();
    }

    public override string ToString()
    {
        return Condition.Value.ToString() ?? true.ToString();
    }

    protected void MakeLoop(Node? node)
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