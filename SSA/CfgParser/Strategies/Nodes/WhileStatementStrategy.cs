using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;
using SSA.CfgParser.Strategies.Variables;

namespace SSA.CfgParser.Strategies.Nodes;

public static class WhileStatementStrategy
{
    public static Node Handle(WhileStatementSyntax syntax)
    {
        var condition = PossibleValueStrategy.Handle(syntax.Condition);
        var whileBlock = syntax.Statement as BlockSyntax;

        return new WhileNode(
            condition!,
            whileBlock is null
                ? null
                : BlockStrategy.Handle(whileBlock));
    }
}