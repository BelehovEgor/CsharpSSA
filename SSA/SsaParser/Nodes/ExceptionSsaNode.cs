using SSA.Common.Models;
using SSA.SsaParser.Nodes.Models;

namespace SSA.SsaParser.Nodes;

public class ExceptionSsaNode : SsaNode
{
    public string Exception { get; init; }

    public ExceptionSsaNode(
        Guid id,
        string exception) 
        : base(id)
    {
        Exception = exception;
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