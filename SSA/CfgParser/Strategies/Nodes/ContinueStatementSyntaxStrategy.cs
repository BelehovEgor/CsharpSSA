using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;

namespace SSA.CfgParser.Strategies.Nodes;

public static class ContinueStatementSyntaxStrategy
{
    public static Node Handle(ContinueStatementSyntax block)
    {
        return new ContinueNode();
    }
}