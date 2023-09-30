using OneOf;
using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

using PossibleValue = OneOf<BinaryExpressionVariable, Variable, string?>;

public sealed class ReturnNode : Node
{
    public required PossibleValue ReturnValue { get; init; }

    public override void AddNext(Node node)
    {
    }

    public override ICollection<Variable> GetNodeVariables()
    {
        return ArraySegment<Variable>.Empty;
    }

    public override string ToString()
    {
        return $"return {ReturnValue.Value.ToString() ?? string.Empty}";
    }
}