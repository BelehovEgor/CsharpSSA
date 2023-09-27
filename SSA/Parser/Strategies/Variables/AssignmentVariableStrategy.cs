using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes.Models;

namespace SSA.Parser.Strategies.Variables;

public static class AssignmentVariableStrategy
{
    public static Variable Handle(AssignmentExpressionSyntax syntax)
    {
        if (syntax.Left is not IdentifierNameSyntax left)
        {
            throw new InvalidOperationException();
        }

        var variable = new Variable(
            left.Identifier.Text, 
            0, 
            PossibleValueStrategy.Handle(syntax.Right).Match<PossibleValue>(
                binaryExpression => binaryExpression,
                variable => variable,
                expression => expression));

        return variable;
    }
}