using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;

namespace SSA.CfgParser.Strategies.Nodes;

public static class ExceptionStatementStrategy
{
    public static Node Handle(ThrowStatementSyntax syntax)
    {
        return new ExceptionNode
        {
            Exception = syntax.ToString()
        };
    }
}