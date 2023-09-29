using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;
using SSA.Nodes.Extensions;
using SSA.Parser.Strategies.Variables;

namespace SSA.Parser.Strategies.Nodes;

public static class ForStatementStrategy
{
    public static INode Handle(ForStatementSyntax syntax)
    {
        var declaration = syntax.Declaration;
        var condition = syntax.Condition;
        var block = syntax.Statement;
        var incrementors = syntax.Incrementors;

        var initValues = new List<PossibleValue>();

        if (declaration is not null)
        {
            var variables = declaration.Variables
                .Select(VariableStrategy.Handle)
                .Select(x => x.Map())
                .ToArray();
            initValues.AddRange(variables);
        }

        var blockNode = BlockStrategy.Handle((BlockSyntax) block);
        var incPossibleValues = 
            incrementors
                .Select(PossibleValueStrategy.Handle)
                .ToArray();
        var incrementorNode = incPossibleValues.Length == 0
            ? null
            : new InitNode
            {
                Variables = incPossibleValues!
            };

        if (incrementorNode is not null)
        {
            var lastNodes = blockNode.GetLastReturnsNodesFromBlock();
            foreach (var lastNode in lastNodes)
            {
                lastNode.AddNext(incrementorNode);
            }
        }
        
        return new ForNode(
            initValues,
            (condition is not null 
                ? PossibleValueStrategy.Handle(condition)
                : true.ToString())!,
            blockNode);
    }
}