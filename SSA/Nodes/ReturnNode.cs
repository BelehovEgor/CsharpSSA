using SSA.Nodes.Models;
using OneOf;

namespace SSA.Nodes;

using PossibleValue = OneOf<BinaryExpressionVariable, Variable, string?>;

public sealed class ReturnNode : NodeBase
{
    public required PossibleValue ReturnValue { get; init; }

    public override void AddNext(params INode[] nodes)
    {
    }

    public override string ToString()
    {
        return $"return {ReturnValue.Value.ToString() ?? string.Empty}";
    }
}