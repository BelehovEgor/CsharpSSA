using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;
using SSA.Parser.Strategies.Variables;

namespace SSA.Parser.Strategies.Nodes;

public static class WhileStatementStrategy
{
    public static INode Handle(WhileStatementSyntax syntax)
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