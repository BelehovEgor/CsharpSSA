using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public sealed class InitNode : Node
{
    public required ICollection<PossibleValue> Variables { get; init; }

    public override void AddNext(Node node)
    {
        AddMember(node);
    }

    public override ICollection<Variable> GetNodeVariables()
    {
        return Variables.SelectMany(x => x.GetAllVariables()).ToArray();
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, Variables.Select(x => x.Value.ToString()));
    }
}