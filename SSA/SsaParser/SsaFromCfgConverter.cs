using SSA.CfgParser.Nodes;
using SSA.Common.Extensions;
using SSA.Common.Models;
using SSA.SsaParser.Nodes;
using SSA.SsaParser.Nodes.Models;

namespace SSA.SsaParser;

public class SsaFromCfgConverter
{
    public SsaNode CreateGraph(Node node)
    {
        var ssaNodes = CreateNodes(node, new HashSet<Guid>()).ToDictionary(x => x.Id);
        
        AddMembers(ssaNodes, node, new HashSet<Guid>());

        var rootSsa = ssaNodes[node.Id];

        var globalVariablesVersions = new Dictionary<string, Variable>();
        SetVersions(rootSsa, new(), globalVariablesVersions);
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

    // public void ChangeForToWhileWithInit(SsaNode node, HashSet<Guid> nodeIds)
    // {
    //     if (nodeIds.Contains(node.Id)) return;
    //
    //     nodeIds.Add(node.Id);
    //
    //     if (node is not ForSsaNode forSsaNode || !forSsaNode.Declarations.Any())
    //     {
    //         foreach (var member in node.Members.ToArray())
    //         {
    //             ChangeForToWhileWithInit(member, nodeIds);
    //         }
    //
    //         return;
    //     }
    //
    //     var initNode = new InitSsaNode(Guid.NewGuid(), forSsaNode.Declarations);
    //     var whileNode = new WhileSsaNode(Guid.NewGuid(), forSsaNode.Condition);
    //     
    //     initNode.AddMember(whileNode);
    //     
    //     
    // }
    
    private ICollection<PhiVariable> GetPhiFunctions(SsaNode node)
    {
        if (node.Parents.Count < 2) return Array.Empty<PhiVariable>();

        var versionDictionaries = node.Parents
            .Select(x => x.VariablesNameVersions)
            .ToList();
        if (node is ForSsaNode forSsaNode) // дикие костыли
        {
            var declarationVariables = forSsaNode.Declarations
                .Where(x => x.IsT1)
                .Select(x => x.AsT1)
                .ToDictionary(x => x.Name, x => x);
            versionDictionaries.Add(declarationVariables);
        }
        
        var commonDiff = Compare(versionDictionaries);

        return commonDiff
            .Select(pair => new PhiVariable(pair.Key, pair.Value))
            .ToArray();
    }

    private void SetVersions(
        SsaNode node, 
        HashSet<Guid> processedNodeIds, 
        Dictionary<string, Variable> globalVariableVersions)
    {
        if (processedNodeIds.Contains(node.Id)) return;

        processedNodeIds.Add(node.Id);

        node.VariablesNameVersions =
            Clone(node.Parents.FirstOrDefault()?.VariablesNameVersions ?? new Dictionary<string, Variable>());  
        SetVersion(node, globalVariableVersions);
        
        foreach (var member in node.Members)
        {
            SetVersions(member, processedNodeIds, globalVariableVersions);
        }
    }

    private void SetVersion(
        SsaNode node,
        Dictionary<string, Variable> globalNameVersions)
    {
        foreach (var variable in node.GetNodeVariables())
        {
            if (variable.HasValue())
            {
                variable.Version = globalNameVersions.TryGetValue(variable.Name, out var prevVariable)
                    ? prevVariable.Version + 1
                    : 0;
                
                globalNameVersions[variable.Name] = variable;
                node.VariablesNameVersions[variable.Name] = variable;
            }
            else
            {
                if (node.VariablesNameVersions.TryGetValue(variable.Name, out var prevVariable))
                {
                    variable.Version = prevVariable.Version;
                }
                else
                {
                    variable.Version = 0;
                }

                node.VariablesNameVersions[variable.Name] = variable;
                globalNameVersions.TryAdd(variable.Name, variable);
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
    
    private Dictionary<string, List<Variable>> Compare(ICollection<Dictionary<string, Variable>> versions)
    {
        var newDict = new Dictionary<string, List<Variable>>();
        foreach (var version in versions)
        {
            foreach (var (name, variable) in version)
            {
                if (newDict.TryGetValue(name, out var set))
                {
                    if (set.All(x => x.Version != variable.Version))
                    {
                        set.Add(variable);
                    }
                }
                else
                {
                    newDict[name] = new List<Variable> { variable };
                }
            }
        }
        
        return newDict
            .Where(x => x.Value.Count > 1)
            .ToDictionary(x => x.Key, x => x.Value);
    }
}