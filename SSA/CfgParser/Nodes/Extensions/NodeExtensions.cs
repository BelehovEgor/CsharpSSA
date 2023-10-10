namespace SSA.CfgParser.Nodes.Extensions;

public static class NodeExtensions
{
    public static IEnumerable<Node> GetLastReturnsNodesFromBlock(this Node node)
    {
        var nodes = GetLastReturnsNodesFromBlock(node, new());

        return nodes;
    }
    
    public static IEnumerable<Node> GetBreakNodes(this WhileNode node)
    {
        if (node.True is null) return ArraySegment<Node>.Empty;
        
        var nodes = GetBreakNodes(node.True, new() {node.Id});

        return nodes;
    }
    
    public static IEnumerable<Node> GetContinueNodes(this WhileNode node)
    {
        if (node.True is null) return ArraySegment<Node>.Empty;
        
        var nodes = GetContinueNodes(node.True, new() {node.Id});

        return nodes;
    }
    
    public static IEnumerable<Node> GetContinueNodes(this Node node, HashSet<Guid> usedNodes)
    {
        if (node is ContinueNode) return new[] {node};
        
        var nodes = new List<Node>();
        foreach (var member in node.Members)
        {
            if (usedNodes.Contains(member.Id)) continue;
            usedNodes.Add(member.Id);
            
            nodes.AddRange(GetContinueNodes(member, usedNodes));
        }

        return nodes;
    }
    
    private static IEnumerable<Node> GetLastReturnsNodesFromBlock(this Node node, HashSet<Guid> usedNodes)
    {
        switch (node)
        {
            case InitNode initNode:
                if (initNode.Members.Count == 0) return new[] {node};
                break;
            case IfNode ifNode:
                if (ifNode.Members.Count < 2) return new[] {node};
                break;
            case ReturnNode:
                return Array.Empty<Node>();
            case ExceptionNode:
                return Array.Empty<Node>();
            case BreakNode:
                return Array.Empty<Node>();
        }
        
        var nodes = new List<Node>();
        foreach (var member in node.Members)
        {
            if (usedNodes.Contains(member.Id)) continue;
            usedNodes.Add(member.Id);
            
            nodes.AddRange(GetLastReturnsNodesFromBlock(member, usedNodes));
        }

        return nodes;
    }
    
    private static IEnumerable<Node> GetBreakNodes(this Node node, HashSet<Guid> usedNodes)
    {
        if (node is BreakNode) return new[] {node};
        
        var nodes = new List<Node>();
        foreach (var member in node.Members)
        {
            if (usedNodes.Contains(member.Id)) continue;
            usedNodes.Add(member.Id);
            
            nodes.AddRange(GetBreakNodes(member, usedNodes));
        }

        return nodes;
    }
}