namespace SSA.Nodes;

public interface INode
{
    Guid Id { get; }
    IReadOnlyCollection<INode> Members { get; }
    void AddPrev(params INode[] nodes);
    void AddNext(params INode[] nodes);
}