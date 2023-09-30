using OneOf;
using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;
using SSA.SsaParser.Nodes.Models;

namespace SSA.SsaParser.Nodes;

using PossibleValue = OneOf<BinaryExpressionVariable, Variable, string?>;

public sealed class ReturnSsaNode : SsaNode
{
    public PossibleValue ReturnValue { get; init; }
    
    public ReturnSsaNode(
        Guid id,
        PossibleValue returnValue) 
        : base(id)
    {
        ReturnValue = returnValue;
    }

    public override ICollection<Variable> GetNodeVariables()
    {
        return PhiVariables.Concat(ReturnValue.GetAllVariables()).ToArray();
    }

    public override string ToString()
    {
        var values = new List<string>();
        var phiString = PhiVariables.Select(x => x.ToString());
        values.AddRange(phiString);
        values.Add($"return {ReturnValue.Value.ToString() ?? string.Empty}");
        return string.Join(Environment.NewLine, values);
    }
}