using System.Text;
using SSA.Common.Models;

namespace SSA.SsaParser.Nodes.Models;

public record PhiVariable(string Name, ICollection<Variable> ConflictedVariables) 
    : Variable(Name, null as string)
{
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"{Name} \u2190 φ( ");

        var argumentsOfFunc = string.Join(", ", ConflictedVariables.Select(x => $"{x.Name}_{x.Version}")); 

        return $"{Name}_{Version} \u2190 φ({argumentsOfFunc})";
    }

    public override bool HasValue()
    {
        return true;
    }
}