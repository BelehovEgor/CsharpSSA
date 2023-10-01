using DotNetGraph.Compilation;
using DotNetGraph.Core;
using DotNetGraph.Extensions;
using SSA.SsaParser.Nodes;

namespace SSA.Visualisation;

public class SsaPainter
{
    public async Task Create(SsaNode node)
    {
        var graph = new DotGraph().WithIdentifier("SSA").Directed();
        
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
        
        await File.WriteAllTextAsync("ssa_graph.dot", result);
    }

    private (List<DotNode> nodes, List<DotEdge> edges) CreateElements(SsaNode node)
    {
        var usedIds = new HashSet<Guid>();
        
        return CreateElementsRec(node, usedIds);
    }
    
    private (List<DotNode> nodes, List<DotEdge> edges) CreateElementsRec(SsaNode node, HashSet<Guid> usedIds)
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

    private DotNode CreateNode(SsaNode node)
    {
        var shape = node switch
        {
            ForSsaNode => DotNodeShape.Octagon,
            WhileSsaNode => DotNodeShape.Hexagon,
            IfSsaNode => DotNodeShape.Diamond,
            ReturnSsaNode => DotNodeShape.Ellipse,
            ExceptionSsaNode => DotNodeShape.Ellipse,
            _ => DotNodeShape.Box
        };
        
        return new DotNode()
            .WithIdentifier(node.Id.ToString())
            .WithShape(shape)
            .WithLabel(node.ToString());
    }

    private DotEdge[] CreateEdges(SsaNode node)
    {
        return node switch
        {
            IfSsaNode => CreateEdgesForConditionNodes(node),
            WhileSsaNode => CreateEdgesForConditionNodes(node),
            _ => node.Members
                .Select(m =>
                    new DotEdge()
                        .From(node.Id.ToString())
                        .To(m.Id.ToString()))
                .ToArray()
        };
    }

    private DotEdge[] CreateEdgesForConditionNodes(SsaNode node)
    {
        var firstEdge =
            new DotEdge()
                .From(node.Id.ToString())
                .To(node.Members.First().Id.ToString())
                .WithColor(DotColor.Green);

        var secondEdge =
            new DotEdge()
                .From(node.Id.ToString())
                .To(node.Members.Last().Id.ToString())
                .WithColor(DotColor.Red);

        return new[] { firstEdge, secondEdge };
    }
}