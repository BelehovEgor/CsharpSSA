using SSA.CfgParser.Nodes.Extensions;
using SSA.Common.Extensions;
using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public class ForNode : WhileNode
{
    public ICollection<PossibleValue>  Declarations { get; init; }

    public ForNode(
        ICollection<PossibleValue> declarations,
        PossibleValue condition,
        Node? trueNode) 
        : base(condition, trueNode)
    {
        Declarations = declarations;
    }
    
    public override string ToString()
    {
        var values = new List<PossibleValue>();
        values.AddRange(Declarations);
        values.Add(Condition);
        
        return string.Join(Environment.NewLine, values.Select(x => x.Value.ToString()));
    }
    
    public override ICollection<Variable> GetNodeVariables()
    {
        var whileVariables = Condition.GetAllVariables();
        var declarationVariables = Declarations.SelectMany(x => x.GetAllVariables()).ToList();
       
        return declarationVariables.Concat(whileVariables).ToArray();
    }
}