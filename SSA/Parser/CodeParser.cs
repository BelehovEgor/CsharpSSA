using Microsoft.CodeAnalysis.CSharp;
using SSA.Nodes;
using SSA.Nodes.Models;
using SSA.Syntax;
using OneOf;
using SSA.Parser.Strategies.Nodes;
using SSA.Parser.Strategies.Variables;

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

        var methodArguments = MethodArgumentStrategy.Handle(method.ParameterList.Parameters);
        
        var methodArgumentsNode = new InitNode
        {
            Variables = methodArguments.Select(x => x.Map()).ToArray()
        };
        
        if (method.Body is not null)
        {
            var bodyTree = BlockStrategy.Handle(method.Body!);
            methodArgumentsNode.AddNext(bodyTree);
        }
        
        return methodArgumentsNode;
    }
}