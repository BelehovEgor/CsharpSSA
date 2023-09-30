using SSA.Common.Models;

namespace SSA.CfgParser.Nodes;

public abstract class Node
{
    private readonly List<Node> _membersList = new();
    private readonly List<Node> _parentsList = new(); 

    public IReadOnlyCollection<Node> Members => _membersList.AsReadOnly();
    public IReadOnlyCollection<Node> Parents => _parentsList.AsReadOnly();
    
    public Guid Id { get; } = Guid.NewGuid();

    public abstract void AddNext(Node node);
    public abstract ICollection<Variable> GetNodeVariables();
    
    protected void AddMember(Node node)
    {
        _membersList.Add(node);
        node._parentsList.Add(this);
    }
}