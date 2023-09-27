using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;
using SSA.Parser.Strategies.Variables;

namespace SSA.Parser.Strategies.Nodes;

public static class AssigmentStrategy
{
    public static INode Handle(AssignmentExpressionSyntax syntax)
    {
        var variable = AssignmentVariableStrategy.Handle(syntax);

        return new InitNode
        {
            Variables = new List<PossibleValue> {variable.Map()}
        };
    }
}