using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes.Models;

namespace SSA.Parser.Strategies.Variables;

public static class VariableStrategy
{
    public static Variable Handle(VariableDeclaratorSyntax syntax)
    {
        var variableName = syntax.Identifier.Text;
        
        var right = syntax.Initializer;
    
        if (right is null) return new Variable(variableName, 0, null as string);

        var value = PossibleValueStrategy.Handle(right.Value);

        return new Variable(
            variableName, 
            0, 
            value.Match<PossibleValue>(
                binaryExpression => binaryExpression,
                variable => variable,
                expression => expression));
    }
}