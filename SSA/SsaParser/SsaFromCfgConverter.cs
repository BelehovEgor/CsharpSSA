using SSA.CfgParser.Nodes;
using SSA.Common.Extensions;
using SSA.Common.Models;
using SSA.SsaParser.Nodes;
using SSA.SsaParser.Nodes.Models;

namespace SSA.SsaParser;

public class SsaFromCfgConverter
{
    private Dictionary<Guid, Dictionary<string, Variable>> _versionOnNodes = new();

    public SsaNode CreateGraph(Node node)
    {
        var ssaNodes = CreateNodes(node, new HashSet<Guid>()).ToDictionary(x => x.Id);
        
        AddMembers(ssaNodes, node, new HashSet<Guid>());

        var rootSsa = ssaNodes[node.Id];
        
        CreatePreliminaryVersions(rootSsa, new());
        foreach (var ssaNode in ssaNodes.Values)
        {
            ssaNode.AddPhiVariables(GetPhiFunctions(ssaNode));
        }

        SetVersions(rootSsa, new(), new());

        return rootSsa;
    }
    
    private ICollection<SsaNode> CreateNodes(Node node, HashSet<Guid> nodeIds)
    {
        if (nodeIds.Contains(node.Id)) return Array.Empty<SsaNode>();

        nodeIds.Add(node.Id);
        
        var ssaNodes = new List<SsaNode>();

        var id = node.Id;
        SsaNode ssaNode = node switch
        {
            ForNode forNode => new ForSsaNode(
                id, 
                forNode.Declarations.Select(x => x.Clone()).ToArray(), 
                forNode.Condition.Clone()),
            WhileNode whileNode => new WhileSsaNode(id, whileNode.Condition.Clone()),
            IfNode ifNode => new IfSsaNode(id, ifNode.Condition.Clone()),
            InitNode initNode => new InitSsaNode(id, initNode.Variables.Select(x => x.Clone()).ToArray()),
            ReturnNode returnNode => new ReturnSsaNode(id, returnNode.ReturnValue.Clone()),
            ExceptionNode exceptionNode => new ExceptionSsaNode(id, exceptionNode.Exception),
            _ => throw new ArgumentOutOfRangeException(nameof(node))
        };
        
        ssaNodes.Add(ssaNode);

        foreach (var member in node.Members)
        {
            ssaNodes.AddRange(CreateNodes(member, nodeIds));
        }

        return ssaNodes;
    }
    
    private void AddMembers(Dictionary<Guid, SsaNode> ssaNodes, Node node, HashSet<Guid> nodeIds)
    {
        if (nodeIds.Contains(node.Id)) return;

        nodeIds.Add(node.Id);

        var currentNode = ssaNodes[node.Id];
        
        foreach (var member in node.Members)
        {
            var memberNode = ssaNodes[member.Id];
            
            currentNode.AddMember(memberNode);

            AddMembers(ssaNodes, member, nodeIds);
        }
    }

    private ICollection<PhiVariable> GetPhiFunctions(SsaNode node)
    {
        if (node.Parents.Count < 2) return Array.Empty<PhiVariable>();

        var nodeVariable = node.GetNodeVariables();
        var currentNodeVersions = _versionOnNodes[node.Id]
            .Where(pair => nodeVariable.Any(var => var.Name == pair.Value.Name))
            .ToDictionary(x => x.Key, x => x.Value);
        var versionDictionaries = _versionOnNodes
            .Where(pair => node.Parents.Any(x => x.Id == pair.Key))
            .ToArray();

        var versionConflicts = new Dictionary<string, List<Variable>>();
        foreach (var versionDictionary in versionDictionaries)
        {
            var diff = Compare(currentNodeVersions, versionDictionary.Value);

            foreach (var diffPair in diff)
            {
                if (versionConflicts.TryGetValue(diffPair.Key, out var list))
                {
                    list.AddRange(diffPair.Value);
                }
                else
                {
                    versionConflicts[diffPair.Key] = diffPair.Value;
                }
            }
        }

        return versionConflicts
            .Select(pair => new PhiVariable(pair.Key, pair.Value))
            .ToArray();
    }
    
    private void CreatePreliminaryVersions(SsaNode node, Dictionary<string, Variable> variablesNameVersions)
    {
        if (_versionOnNodes.ContainsKey(node.Id)) return;

        SetVersion(node.GetNodeVariables(), variablesNameVersions);
        
        _versionOnNodes.Add(node.Id, Clone(variablesNameVersions));
                
        foreach (var member in node.Members)
        {
            CreatePreliminaryVersions(member, variablesNameVersions);
        }
    }

    private void SetVersions(
        SsaNode node, 
        HashSet<Guid> processedNodeIds, 
        Dictionary<string, Variable> variablesNameVersions)
    {
        if (processedNodeIds.Contains(node.Id)) return;

        processedNodeIds.Add(node.Id);
        
        SetVersion(node.GetNodeVariables(), variablesNameVersions);
        
        foreach (var member in node.Members)
        {
            SetVersions(member, processedNodeIds, variablesNameVersions);
        }
    }

    private void SetVersion(ICollection<Variable> variables, Dictionary<string, Variable> variablesNameVersions)
    {
        foreach (var variable in variables)
        {
            if (variable.HasValue())
            {
                variable.Version = variablesNameVersions.TryGetValue(variable.Name, out var prevVariable)
                    ? prevVariable.Version + 1
                    : 0;
                
                variablesNameVersions[variable.Name] = variable;
            }
            else
            {
                if (variablesNameVersions.TryGetValue(variable.Name, out var prevVariable))
                {
                    variable.Version = prevVariable.Version;
                }
                else
                {
                    variable.Version = 0;
                    
                    variablesNameVersions[variable.Name] = variable;
                }
            }
        }
    }

    private Dictionary<TKey, TValue> Clone<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        var newDict = new Dictionary<TKey, TValue>();
        foreach (var (key, value) in dictionary)
        {
            newDict.Add(key, value);
        }

        return newDict;
    }
    
    private Dictionary<string, List<Variable>> Compare(Dictionary<string, Variable> first, Dictionary<string, Variable> second)
    {
        var newDict = new Dictionary<string, List<Variable>>();
        foreach (var (name, firstVariable) in first)
        {
            if (second.TryGetValue(name, out var secondVariable) && firstVariable.Version != secondVariable.Version)
            {
                newDict.Add(name, new() {firstVariable, secondVariable});
            }
        }

        return newDict;
    }
}