using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Syntax.Models;
using OneOf;

namespace SSA.Syntax;

using PossibleValue = OneOf<BinaryExpression, LiteralExpressionSyntax, IdentifierNameSyntax>;

public static class SyntaxExtensions
{
    public static MethodDeclarationSyntax GetMethodByName(this ClassDeclarationSyntax rootClass, string methodName)
    {
        return rootClass.Members
            .Select(x => x as MethodDeclarationSyntax)
            .Single(x =>
                x is not null &&
                x.Identifier.ValueText == methodName)!;
    }

    public static ClassDeclarationSyntax GetClassByName(this NamespaceDeclarationSyntax rootNamespace, string className)
    {
        return rootNamespace.Members
            .Select(x => x as ClassDeclarationSyntax)
            .Single(x => 
                x is not null &&
                x.Identifier.ValueText == className)!;
    }

    public static NamespaceDeclarationSyntax GetNamespaceByName(this SyntaxTree tree, string namespaceName)
    {
        var root = tree.GetCompilationUnitRoot();

        return root.Members
            .Select(x => x as NamespaceDeclarationSyntax)
            .Single(x =>
                x is not null &&
                x.Name.ToString() == namespaceName)!;
    }
    
    public static BinaryExpression ToBinaryExpression(this BinaryExpressionSyntax syntax)
    {
        var left = syntax.Left;
        var right = syntax.Right;

        return new BinaryExpression
        {
            Left = left.Match(),
            Right = right.Match(),
            Operation = syntax.OperatorToken
        };
    }

    private static PossibleValue Match(this ExpressionSyntax syntax)
    {
        return syntax switch
        {
            BinaryExpressionSyntax binaryExpressionSyntaxLeft 
                => binaryExpressionSyntaxLeft.ToBinaryExpression(),
            ParenthesizedExpressionSyntax parenthesizedExpressionSyntax 
                => parenthesizedExpressionSyntax.ToParenthesizedBinaryExpression(),
            LiteralExpressionSyntax literalExpressionSyntax 
                => literalExpressionSyntax,
            IdentifierNameSyntax identifierNameSyntax 
                => identifierNameSyntax,
            _ => throw new InvalidOperationException()
        };
    }
    
    private static PossibleValue ToParenthesizedBinaryExpression(this ParenthesizedExpressionSyntax syntax)
    {
        var possibleValue = syntax.Expression.Match();

        return possibleValue.Match<PossibleValue>(
            binaryExpression => new ParenthesizedBinaryExpression
            {
                Left = binaryExpression.Left,
                Right = binaryExpression.Right,
                Operation = binaryExpression.Operation
            },
            literal => literal,
            identifier => identifier);
    }
}