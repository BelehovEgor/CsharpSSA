using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public class ExceptionNode : Node
{
    public required string Exception { get; init; }

    public override void AddNext(Node node)
    {
    }

    public override ICollection<Variable> GetNodeVariables()
    {
        return Array.Empty<Variable>();
    }

    public override string ToString()
    {
        return Exception;
    }
}