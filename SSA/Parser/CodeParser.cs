using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Nodes;
using SSA.Nodes.Models;
using SSA.Syntax;
using OneOf;
using SSA.Syntax.Models;

namespace SSA.Parser;

using PossibleValue = OneOf<BinaryExpressionVariable, Variable, string?>;

public class CodeParser
{
    public INode ParseMethod(
        string programCode,
        string namespaceName,
        string className,
        string methodName)
    {
        var method = CSharpSyntaxTree.ParseText(programCode)
            .GetNamespaceByName(namespaceName)
            .GetClassByName(className)
            .GetMethodByName(methodName);

        var methodArguments = GetMethodArguments(method.ParameterList.Parameters);
        var methodArgumentsNode = new InitNode
        {
            Variables = methodArguments
        };
        
        if (method.Body is not null)
        {
            var bodyTree = ParseBlockSyntax(method.Body!);
            methodArgumentsNode.AddNext(bodyTree);
        }
        
        return methodArgumentsNode;
    }

    private INode ParseBlockSyntax(BlockSyntax block)
    {
        INode? rootNode = null;
        INode? currentNode = null;
        
        foreach (var syntax in block.Statements)
        {
            var node = syntax switch
            {
                LocalDeclarationStatementSyntax localDeclarationStatementSyntax =>
                    ParseLocalDeclarationStatementSyntax(localDeclarationStatementSyntax),
                ExpressionStatementSyntax expressionStatementSyntax =>
                    ParseExpressionStatementSyntax(expressionStatementSyntax),
                ReturnStatementSyntax returnStatementSyntax =>
                    ParseReturnStatementSyntax(returnStatementSyntax),
                IfStatementSyntax ifStatementSyntax =>
                    ParseIfStatementSyntax(ifStatementSyntax),
                _ => throw new InvalidOperationException($"I can't parse {syntax}.")
            };

            if (rootNode is null)
            {
                rootNode = node;
            }

            currentNode?.AddNext(node);
            currentNode = node;
        }

        return rootNode!;
    }
    
    private INode ParseIfStatementSyntax(IfStatementSyntax syntax)
    {
        var condition = MatchExpressionSyntax(syntax.Condition);
        var ifBlock = syntax.Statement as BlockSyntax;
        var elseBlock = syntax.Else?.Statement as BlockSyntax;

        return new IfNode(
            condition!,
            ifBlock is null
                ? null
                : ParseBlockSyntax(ifBlock),
            elseBlock is null
                ? null
                : ParseBlockSyntax(elseBlock));
    }
    
    private INode ParseReturnStatementSyntax(ReturnStatementSyntax syntax)
    {
        var nodeExpression = syntax.Expression;
        
        var value = nodeExpression is null
            ? null as string
            : MatchExpressionSyntax(nodeExpression);

        return new ReturnNode
        {
            ReturnValue = value!
        };
    }
    
    private INode ParseExpressionStatementSyntax(ExpressionStatementSyntax syntax)
    {
        var nodeExpression = syntax.Expression;

        var node = nodeExpression switch
        {
            AssignmentExpressionSyntax assignmentExpressionSyntax =>
                ParseAssignmentExpressionSyntax(assignmentExpressionSyntax),
            _ => throw new InvalidOperationException($"I can't parse {nodeExpression}.")
        };

        return node;
    }
    
    private InitNode ParseLocalDeclarationStatementSyntax(LocalDeclarationStatementSyntax syntax)
    {
        var variables = syntax.Declaration.Variables
            .Select(ParseVariableDeclaratorSyntax)
            .ToArray();

        return new InitNode
        {
            Variables = variables
        };
    }
    
    private Variable ParseVariableDeclaratorSyntax(VariableDeclaratorSyntax syntax)
    {
        var variableName = syntax.Identifier.Text;
        
        var right = syntax.Initializer;
    
        if (right is null) return new Variable(variableName, 0, null as string);

        var value = MatchExpressionSyntax(right.Value);

        return new Variable(
            variableName, 
            0, 
            value.Match<PossibleValue>(
                binaryExpression => binaryExpression,
                variable => variable,
                expression => expression));
    }
    
    private INode ParseAssignmentExpressionSyntax(AssignmentExpressionSyntax syntax)
    {
        if (syntax.Left is not IdentifierNameSyntax left)
        {
            throw new InvalidOperationException();
        }

        var variable = new Variable(
            left.Identifier.Text, 
            0, 
            MatchExpressionSyntax(syntax.Right).Match<PossibleValue>(
                binaryExpression => binaryExpression,
                variable => variable,
                expression => expression));

        return new InitNode
        {
            Variables = new List<Variable> {variable}
        };
    }

    private OneOf<BinaryExpressionVariable, Variable, string> MatchExpressionSyntax(ExpressionSyntax syntax)
    {
        switch (syntax)
        {
            case LiteralExpressionSyntax literalExpressionSyntax:
                return literalExpressionSyntax.MapToExpression();
            case IdentifierNameSyntax identifierNameSyntax:
                return identifierNameSyntax.MapToVariable();
            case BinaryExpressionSyntax binaryExpressionSyntax:
                var expression = binaryExpressionSyntax.ToBinaryExpression();
                return expression.MapToVariable();
            default:
                throw new InvalidOperationException($"I can't parse {syntax}.");
        }
    }
    
    private Variable[] GetMethodArguments(IEnumerable<ParameterSyntax> parameters)
    {
        return parameters.Select(GetVariableFromParameter).ToArray();
    }
    
    private Variable GetVariableFromParameter(ParameterSyntax parameterSyntax)
    {
        var name = parameterSyntax.Identifier.ValueText;

        return new Variable(name, 0, null as string);
    }
}

public static class Mapper
{
    public static BinaryExpressionVariable MapToVariable(this BinaryExpression expression)
    {
        var left = expression.Left;
        var right = expression.Right;

        var leftValue = left.Map();
        var rightValue = right.Map();
        var operationText = expression.Operation.Text;

        return expression is ParenthesizedBinaryExpression
            ? new ParenthesizedBinaryExpressionVariable
            {
                Left = leftValue,
                Right = rightValue,
                Operation = operationText
            }
            : new BinaryExpressionVariable
            {
                Left = leftValue,
                Right = rightValue,
                Operation = operationText
            };
    }

    public static string MapToExpression(this LiteralExpressionSyntax syntax)
        => syntax.ToString();

    public static Variable MapToVariable(this IdentifierNameSyntax syntax) 
        => new(syntax.Identifier.Text, 0, null as string);
    
    private static OneOf<BinaryExpressionVariable, Variable, string> Map(
        this OneOf<BinaryExpression, LiteralExpressionSyntax, IdentifierNameSyntax> oneOf)
        => oneOf.Match<OneOf<BinaryExpressionVariable, Variable, string>>(
            binaryExpressionSyntax => binaryExpressionSyntax.MapToVariable(),
            literalExpressionSyntax => literalExpressionSyntax.MapToExpression(),
            identifierNameSyntax => identifierNameSyntax.MapToVariable());
}