using Microsoft.CodeAnalysis.CSharp.Syntax;
using OneOf;
using SSA.Common.Models;
using SSA.Syntax;
using SSA.Syntax.Models;

namespace SSA.CfgParser.Strategies.Variables;

public static class PossibleValueStrategy
{
    public static OneOf<BinaryExpressionVariable, Variable, string> Handle(
        ExpressionSyntax syntax)
    {
        switch (syntax)
        {
            case LiteralExpressionSyntax literalExpressionSyntax:
                return literalExpressionSyntax.MapToExpression();
            case IdentifierNameSyntax identifierNameSyntax:
                return identifierNameSyntax.MapToVariable();
            case BinaryExpressionSyntax binaryExpressionSyntax:
                var expression = binaryExpressionSyntax.ToBinaryExpression();
                return expression.MapToVariable();
            case AssignmentExpressionSyntax assignmentExpressionSyntax:
                return AssignmentVariableStrategy.Handle(assignmentExpressionSyntax);
            default:
                throw new InvalidOperationException($"I can't parse {syntax}.");
        }
    }

    private static BinaryExpressionVariable MapToVariable(this BinaryExpression expression)
    {
        var left = expression.Left;
        var right = expression.Right;

        var leftValue = left.Map();
        var rightValue = right.Map();
        var operationText = expression.Operation.Text;

        return expression is ParenthesizedBinaryExpression
            ? new ParenthesizedBinaryExpressionVariable
            {
                Left = leftValue,
                Right = rightValue,
                Operation = operationText
            }
            : new BinaryExpressionVariable
            {
                Left = leftValue,
                Right = rightValue,
                Operation = operationText
            };
    }

    private static string MapToExpression(this LiteralExpressionSyntax syntax)
        => syntax.ToString();

    private static Variable MapToVariable(this IdentifierNameSyntax syntax) 
        => new(syntax.Identifier.Text, null as string);
    
    private static OneOf<BinaryExpressionVariable, Variable, string> Map(
        this OneOf<BinaryExpression, LiteralExpressionSyntax, IdentifierNameSyntax> oneOf)
        => oneOf.Match<OneOf<BinaryExpressionVariable, Variable, string>>(
            binaryExpressionSyntax => binaryExpressionSyntax.MapToVariable(),
            literalExpressionSyntax => literalExpressionSyntax.MapToExpression(),
            identifierNameSyntax => identifierNameSyntax.MapToVariable());
}