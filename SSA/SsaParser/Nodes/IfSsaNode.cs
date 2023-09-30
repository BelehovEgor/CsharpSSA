using OneOf;
using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;

namespace SSA.SsaParser.Nodes;

using PossibleValue = OneOf<BinaryExpressionVariable, Variable, string?>;

public sealed class IfSsaNode : SsaNode
{
    public PossibleValue Condition { get; init; }

    public IfSsaNode(
        Guid id,
        PossibleValue condition) 
        : base(id)
    {
        Condition = condition;
    }
    
    public override ICollection<Variable> GetNodeVariables()
    {
        return PhiVariables.Concat(Condition.GetAllVariables()).ToArray();
    }

    public override string ToString()
    {
        var values = new List<string>();
        var phiString = PhiVariables.Select(x => x.ToString());
        var conditionString = Condition.Value.ToString();
        
        values.AddRange(phiString);
        
        if (conditionString is not null) values.Add(conditionString);
        
        return string.Join(Environment.NewLine, values);
    }
}