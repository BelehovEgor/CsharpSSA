using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;

namespace SSA.Parser.Strategies.Nodes;

public static class ExpressionStatementStrategy
{
    public static INode Handle(ExpressionStatementSyntax syntax)
    {
        var nodeExpression = syntax.Expression;

        var node = nodeExpression switch
        {
            AssignmentExpressionSyntax assignmentExpressionSyntax =>
                AssigmentStrategy.Handle(assignmentExpressionSyntax),
            _ => throw new InvalidOperationException($"I can't parse {nodeExpression}.")
        };

        return node;
    }
}