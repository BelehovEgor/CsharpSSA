using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;
using SSA.Parser.Strategies.Variables;

namespace SSA.Parser.Strategies.Nodes;

public static class IfStatementStrategy
{
    public static INode Handle(IfStatementSyntax syntax)
    {
        var condition = PossibleValueStrategy.Handle(syntax.Condition);
        var ifBlock = syntax.Statement as BlockSyntax;
        var elseBlock = syntax.Else?.Statement as BlockSyntax;

        return new IfNode(
            condition!,
            ifBlock is null
                ? null
                : BlockStrategy.Handle(ifBlock),
            elseBlock is null
                ? null
                : BlockStrategy.Handle(elseBlock));
    }
}