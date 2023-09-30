using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;

namespace SSA.CfgParser.Strategies.Nodes;

public static class ExpressionStatementStrategy
{
    public static Node Handle(ExpressionStatementSyntax syntax)
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