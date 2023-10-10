using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;
using SSA.SsaParser.Nodes.Models;

namespace SSA.SsaParser.Nodes;

public class ForSsaNode : WhileSsaNode
{
    public ForSsaNode(
        Guid id,
        PossibleValue condition) 
        : base(id, condition)
    {
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
    
    public override ICollection<Variable> GetNodeVariables()
    {
        var whileVariables = Condition.GetAllVariables();

        return PhiVariables.Concat(whileVariables).ToArray();
    }
}