using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OneOf;

namespace SSA.Syntax.Models;

using PossibleValue = OneOf<BinaryExpression, LiteralExpressionSyntax, IdentifierNameSyntax>;

public class BinaryExpression
{
    public required PossibleValue Left { get; init; }
    public required PossibleValue Right { get; init; }
    
    public required SyntaxToken Operation { get; init; }
}