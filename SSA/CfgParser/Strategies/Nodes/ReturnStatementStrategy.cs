using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;
using SSA.CfgParser.Strategies.Variables;

namespace SSA.CfgParser.Strategies.Nodes;

public static class ReturnStatementStrategy
{
    public static Node Handle(ReturnStatementSyntax syntax)
    {
        var nodeExpression = syntax.Expression;
        
        var value = nodeExpression is null
            ? null as string
            : PossibleValueStrategy.Handle(nodeExpression);

        return new ReturnNode
        {
            ReturnValue = value!
        };
    }
}