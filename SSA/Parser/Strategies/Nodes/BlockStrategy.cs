using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;
using SSA.Nodes.Models;

namespace SSA.Parser.Strategies.Nodes;

public static class BlockStrategy
{
    public static INode Handle(BlockSyntax block)
    {
        INode? rootNode = null;
        INode? currentNode = null;
        
        foreach (var syntax in block.Statements)
        {
            var node = StatementStrategy.Handle(syntax);

            rootNode ??= node;

            currentNode?.AddNext(node);
            currentNode = node;
        }

        return rootNode ?? 
               new InitNode
               {
                   Variables = Array.Empty<PossibleValue>()
               };
    }
}