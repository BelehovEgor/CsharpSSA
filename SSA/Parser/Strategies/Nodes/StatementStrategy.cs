﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;

namespace SSA.Parser.Strategies.Nodes;

public static class StatementStrategy
{
    public static INode Handle(StatementSyntax syntax)
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
            _ => throw new InvalidOperationException($"I can't parse {syntax}.")
        };
    } 
}