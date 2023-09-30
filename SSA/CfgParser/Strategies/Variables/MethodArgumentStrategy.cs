using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSA.Common.Models;

namespace SSA.CfgParser.Strategies.Variables;

public static class MethodArgumentStrategy
{
    public static Variable[] Handle(IEnumerable<ParameterSyntax> parameters)
    {
        return parameters.Select(GetVariableFromParameter).ToArray();
    }
    
    private static Variable GetVariableFromParameter(ParameterSyntax parameterSyntax)
    {
        var name = parameterSyntax.Identifier.ValueText;

        return new Variable(name, null as string);
    }
}