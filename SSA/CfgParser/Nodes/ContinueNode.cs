using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public class ContinueNode : Node
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
        return "continue";
    }
}