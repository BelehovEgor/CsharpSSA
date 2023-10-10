using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;

namespace SSA.CfgParser.Strategies.Nodes;

public static class BlockStrategy
{
    public static Node Handle(BlockSyntax block)
    {
        Node? rootNode = null;
        Node? currentNode = null;
        
        foreach (var syntax in block.Statements)
        {
            var node = StatementStrategy.Handle(syntax);

            if (node is InitNode initNode && initNode.Members.FirstOrDefault() is ForNode forNode)
            {
                rootNode ??= initNode;
                node = forNode;
            }
            else
            {
                rootNode ??= node;
            }

            if (node is not null)
            {
                currentNode?.AddNext(node);
            }
            currentNode = node;
        }

        return rootNode ?? 
               new InitNode
               {
                   Variables = Array.Empty<PossibleValue>()
               };
    }
}