using Microsoft.CodeAnalysis.CSharp;
using SSA.CfgParser.Nodes;
using SSA.CfgParser.Strategies.Nodes;
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
        
        return BlockStrategy.Handle(method.Body!);
    }
}