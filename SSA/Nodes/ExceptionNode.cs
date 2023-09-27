namespace SSA.Nodes;

public class ExceptionNode : NodeBase
{
    public required string Exception { get; init; }

    public override void AddNext(params INode[] nodes)
    {
    }

    public override string ToString()
    {
        return Exception;
    }
}