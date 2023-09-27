using SSA.Nodes.Models;

namespace SSA.Nodes;

public sealed class InitNode : NodeBase
{
    public required ICollection<PossibleValue> Variables { get; init; }

    public override void AddNext(params INode[] nodes)
    {
        if (nodes.Length != 1)
        {
            throw new ArgumentException("Expected only 1 next node");
        }
        
        MembersList.Add(nodes[0]);
    }

    public override string ToString()
    {
        return string.Join(Environment.NewLine, Variables.Select(x => x.Value.ToString()));
    }
}