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
    
    protected internal void AddMember(Node node)
    {
        _membersList.Add(node);
        node._parentsList.Add(this);
    }
    
    protected internal void DeleteMember(Node node)
    {
        _membersList.Remove(node);
        node._parentsList.Remove(this);
    }

    protected internal void SwapMember(Node prev, Node curr)
    {
        for (int i = 0; i < _membersList.Count; i++)
        {
            if (_membersList[i] == prev)
            {
                _membersList[i] = curr;
                prev._parentsList.Remove(this);
            }
        }
    }
}