using SSA.Common.Models;
using SSA.SsaParser.Nodes.Models;

namespace SSA.SsaParser.Nodes;

public abstract class SsaNode
{
    private readonly List<SsaNode> _membersList = new();
    private readonly List<SsaNode> _parentsList = new();
    private readonly List<PhiVariable> _phiVariablesList = new();
    
    public Guid Id { get; }
    public IReadOnlyCollection<SsaNode> Members => _membersList.AsReadOnly();
    public IReadOnlyCollection<SsaNode> Parents => _parentsList.AsReadOnly();
    public IReadOnlyCollection<PhiVariable> PhiVariables => _phiVariablesList.AsReadOnly();

    public Dictionary<string, Variable> VariablesNameVersions { get; set; } = new();
    
    public SsaNode(Guid id)
    {
        Id = id;
    }

    public void AddMember(SsaNode node)
    {
        _membersList.Add(node);
        node._parentsList.Add(this);
    }
    
    public void RemoveMember(SsaNode node)
    {
        _membersList.Remove(node);
        node._parentsList.Remove(this);
    }
    
    public void AddPhiVariables(ICollection<PhiVariable> phiVariables)
    {
        _phiVariablesList.AddRange(phiVariables);
    }
    
    public abstract ICollection<Variable> GetNodeVariables();
}