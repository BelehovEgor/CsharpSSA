using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;
using SSA.SsaParser.Nodes.Models;

namespace SSA.SsaParser.Nodes;

public class ForSsaNode : WhileSsaNode
{
    public ICollection<PossibleValue> Declarations { get; init; }

    public ForSsaNode(
        Guid id,
        ICollection<PossibleValue> declarations,
        PossibleValue condition) 
        : base(id, condition)
    {
        Declarations = declarations;
    }
    
    public override string ToString()
    {
        var values = new List<string>();

        var declarationString = Declarations
            .Select(x => x.Value.ToString())
            .Where(x => x is not null);
        var phiString = PhiVariables.Select(x => x.ToString());
        var conditionString = Condition.Value.ToString();
        
        values.AddRange(declarationString!);
        values.AddRange(phiString);
        
        if (conditionString is not null) values.Add(conditionString);
        
        return string.Join(Environment.NewLine, values);
    }
    
    public override ICollection<Variable> GetNodeVariables()
    {
        var declarationVariables = Declarations.SelectMany(x => x.GetAllVariables()).ToList();
        var whileVariables = Condition.GetAllVariables();

        return declarationVariables.Concat(PhiVariables).Concat(whileVariables).ToArray();
    }
}