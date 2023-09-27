using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;

namespace SSA.Parser.Strategies.Nodes;

public static class ExceptionStatementStrategy
{
    public static INode Handle(ThrowStatementSyntax syntax)
    {
        return new ExceptionNode
        {
            Exception = syntax.ToString()
        };
    }
}