using SSA.Common.Models;

namespace SSA.SsaParser.Nodes;

public class BreakSsaNode : SsaNode
{
    public BreakSsaNode(Guid id) 
        : base(id)
    {
    }
    
    public override ICollection<Variable> GetNodeVariables()
    {
        return Array.Empty<Variable>();
    }

    public override string ToString()
    {
        return "break";
    }
}