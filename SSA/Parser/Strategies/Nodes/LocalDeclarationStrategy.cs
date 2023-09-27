using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;
using SSA.Parser.Strategies.Variables;

namespace SSA.Parser.Strategies.Nodes;

public static class LocalDeclarationStrategy
{
    public static INode Handle(
        LocalDeclarationStatementSyntax syntax)
    {
        var variables = syntax.Declaration.Variables
            .Select(VariableStrategy.Handle)
            .Select(x => x.Map())
            .ToArray();

        return new InitNode
        {
            Variables = variables
        };
    }
}