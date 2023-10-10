using SSA.Common.Extensions;
using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public class ForNode : WhileNode
{
    public ForNode(
        PossibleValue condition,
        Node? trueNode) 
        : base(condition, trueNode)
    {
    }
    
    public override string ToString()
    {
        var values = new List<PossibleValue>();
        values.Add(Condition);
        
        return string.Join(Environment.NewLine, values.Select(x => x.Value.ToString()));
    }
    
    public override ICollection<Variable> GetNodeVariables()
    {
        return Condition.GetAllVariables();
    }

    public override void AddNext(Node node)
    {
        AddMember(node);

        MakeLoop(True);

        ProcessBreaks(node);
    }
}