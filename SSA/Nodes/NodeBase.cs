namespace SSA.Nodes;

public abstract class NodeBase : INode
{
    protected readonly List<INode> MembersList = new();

    public IReadOnlyCollection<INode> Members => MembersList.AsReadOnly();

    public Guid Id { get; } = Guid.NewGuid();

    public void AddPrev(params INode[] nodes)
    {
        throw new NotImplementedException();
    }

    public abstract void AddNext(params INode[] nodes);
}