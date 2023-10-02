using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public class BreakNode : Node
{
    public override void AddNext(Node node)
    {
        AddMember(node);
    }

    public override ICollection<Variable> GetNodeVariables()
    {
        return ArraySegment<Variable>.Empty;
    }

    public override string ToString()
    {
        return "break";
    }
}