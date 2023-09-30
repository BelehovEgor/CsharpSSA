using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;
using SSA.SsaParser.Nodes.Models;

namespace SSA.SsaParser.Nodes;

public sealed class InitSsaNode : SsaNode
{
    public ICollection<PossibleValue> Variables { get; init; }

    public InitSsaNode(
        Guid id,
        ICollection<PossibleValue> variables) 
        : base(id)
    {
        Variables = variables;
    }
    
    public override ICollection<Variable> GetNodeVariables()
    {
        return PhiVariables.Concat(Variables.SelectMany(x => x.GetAllVariables())).ToArray();
    }

    public override string ToString()
    {
        var values = new List<string>();

        var phiString = PhiVariables.Select(x => x.ToString());
        var variablesString = Variables
            .Select(x => x.Value.ToString())
            .Where(x => x is not null);
        
        values.AddRange(phiString);
        values.AddRange(variablesString!);
        
        return string.Join(Environment.NewLine, values);
    }
}