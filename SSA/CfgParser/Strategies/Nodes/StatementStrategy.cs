using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.CfgParser.Nodes;

namespace SSA.CfgParser.Strategies.Nodes;

public static class StatementStrategy
{
    public static Node? Handle(StatementSyntax syntax)
    {
        return syntax switch
        {
            LocalDeclarationStatementSyntax localDeclarationStatementSyntax =>
                LocalDeclarationStrategy.Handle(localDeclarationStatementSyntax),
            ExpressionStatementSyntax expressionStatementSyntax =>
                ExpressionStatementStrategy.Handle(expressionStatementSyntax),
            ReturnStatementSyntax returnStatementSyntax =>
                ReturnStatementStrategy.Handle(returnStatementSyntax),
            IfStatementSyntax ifStatementSyntax =>
                IfStatementStrategy.Handle(ifStatementSyntax),
            WhileStatementSyntax whileStatementSyntax =>
                WhileStatementStrategy.Handle(whileStatementSyntax),
            ForStatementSyntax forStatementSyntax =>
                ForStatementStrategy.Handle(forStatementSyntax),
            ThrowStatementSyntax throwStatementSyntax =>
                ExceptionStatementStrategy.Handle(throwStatementSyntax),
            BreakStatementSyntax breakStatementSyntax =>
                BreakStatementSyntaxStrategy.Handle(breakStatementSyntax),
            ContinueStatementSyntax continueStatementSyntax =>
                ContinueStatementSyntaxStrategy.Handle(continueStatementSyntax),
            _ => throw new InvalidOperationException($"I can't parse {syntax}.")
        };
    } 
}