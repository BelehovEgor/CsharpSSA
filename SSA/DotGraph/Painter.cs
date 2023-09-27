using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using SSA.Nodes;

namespace SSA.DotGraph;

public class Painter
{
    public async Task Create(INode node)
    {
        var graph = new DotNetGraph.Core.DotGraph().WithIdentifier("SSA").Directed();
        
        var (nodes, edges) = CreateElements(node);

        foreach (var dotNode in nodes)
        {
            graph.Add(dotNode);
        }

        foreach (var dotEdge in edges)
        {
            graph.Add(dotEdge);
        }
        
        await using var writer = new StringWriter();
        var context = new CompilationContext(writer, new CompilationOptions());
        await graph.CompileAsync(context);

        var result = writer.GetStringBuilder().ToString();
        
        File.WriteAllText("graph.dot", result);
    }

    private (List<DotNode> nodes, List<DotEdge> edges) CreateElements(INode node)
    {
        var usedIds = new HashSet<Guid>();
        
        return CreateElementsRec(node, usedIds);
    }
    
    private (List<DotNode> nodes, List<DotEdge> edges) CreateElementsRec(INode node, HashSet<Guid> usedIds)
    {
        var nodes = new List<DotNode>();
        var edges = new List<DotEdge>();

        if (usedIds.Contains(node.Id)) return (nodes, edges);
        
        usedIds.Add(node.Id);
        nodes.Add(CreateNode(node));
        edges.AddRange(CreateEdges(node));

        foreach (var member in node.Members)
        {
            var (memberNodes, memberEdges) = CreateElementsRec(member, usedIds);
            
            nodes.AddRange(memberNodes);
            edges.AddRange(memberEdges);
        }

        return (nodes, edges);
    }

    private DotNode CreateNode(INode node)
    {
        return new DotNode()
            .WithIdentifier(node.Id.ToString())
            .WithShape(DotNodeShape.Box)
            .WithLabel(node.ToString());
    }

    private DotEdge[] CreateEdges(INode node)
    {
        return node.Members
            .Select(m => 
                new DotEdge()
                    .From(node.Id.ToString())
                    .To(m.Id.ToString()))
            .ToArray();
    }
}