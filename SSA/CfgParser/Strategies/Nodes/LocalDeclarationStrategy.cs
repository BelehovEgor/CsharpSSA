using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;
using SSA.CfgParser.Strategies.Variables;

namespace SSA.CfgParser.Strategies.Nodes;

public static class LocalDeclarationStrategy
{
    public static Node? Handle(
        LocalDeclarationStatementSyntax syntax)
    {
        var variables = syntax.Declaration.Variables
            .Select(VariableStrategy.Handle)
            .Where(x => x.HasValue())
            .Select(x => x.Map())
            .ToArray();

        
        return variables.Any()
            ? new InitNode
            {
                Variables = variables
            }
            : null            ;
    }
}