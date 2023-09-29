namespace SSA.Nodes;

public class ForNode : WhileNode
{
    public ICollection<PossibleValue>  Declarations { get; init; }

    public ForNode(ICollection<PossibleValue> declarations, PossibleValue condition, INode? trueNode) 
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
}