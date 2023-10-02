using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;

namespace SSA.CfgParser.Strategies.Nodes;

public static class BreakStatementSyntaxStrategy
{
    public static Node Handle(BreakStatementSyntax block)
    {
        return new BreakNode();
    }
}