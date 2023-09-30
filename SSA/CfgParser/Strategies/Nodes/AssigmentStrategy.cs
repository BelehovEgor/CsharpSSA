using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;
using SSA.CfgParser.Strategies.Variables;

namespace SSA.CfgParser.Strategies.Nodes;

public static class AssigmentStrategy
{
    public static Node Handle(AssignmentExpressionSyntax syntax)
    {
        var variable = AssignmentVariableStrategy.Handle(syntax);

        return new InitNode
        {
            Variables = new List<PossibleValue> {variable.Map()}
        };
    }
}