using Microsoft.CodeAnalysis.CSharp;
using SSA.CfgParser.Nodes;
using SSA.CfgParser.Strategies.Nodes;
using SSA.CfgParser.Strategies.Variables;
using SSA.Syntax;

namespace SSA.CfgParser;

public class CodeParser
{
    public Node ParseMethod(
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