namespace SSA.CfgParser.Nodes.Extensions;

public static class NodeExtensions
{
    public static IEnumerable<Node> GetLastReturnsNodesFromBlock(this Node node)
    {
        var usedNodes = new HashSet<Guid>();
        var nodes = GetLastReturnsNodesFromBlockRec(node, usedNodes);

        return nodes;
    }
    
    private static IEnumerable<Node> GetLastReturnsNodesFromBlockRec(this Node node, HashSet<Guid> usedNodes)
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
        };
        
        var nodes = new List<Node>();
        foreach (var member in node.Members)
        {
            if (usedNodes.Contains(member.Id)) continue;

            usedNodes.Add(member.Id);
            nodes.AddRange(GetLastReturnsNodesFromBlockRec(member, usedNodes));
        }

        return nodes;
    }
}