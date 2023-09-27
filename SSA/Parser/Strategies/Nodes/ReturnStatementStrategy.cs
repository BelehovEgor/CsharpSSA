using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;
using SSA.Parser.Strategies.Variables;

namespace SSA.Parser.Strategies.Nodes;

public static class ReturnStatementStrategy
{
    public static INode Handle(ReturnStatementSyntax syntax)
    {
        var nodeExpression = syntax.Expression;
        
        var value = nodeExpression is null
            ? null as string
            : PossibleValueStrategy.Handle(nodeExpression);

        return new ReturnNode
        {
            ReturnValue = value!
        };
    }
}