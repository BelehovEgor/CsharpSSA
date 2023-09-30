using SSA.Common.Models;

namespace SSA.Common.Extensions;

public static class PossibleValueExtensions
{
    public static ICollection<Variable> GetAllVariables(this PossibleValue value)
    {
        if (value.IsT2) return Array.Empty<Variable>();

        var variables = new List<Variable>();
        
        if (value.IsT1)
        {
            var variable = value.AsT1;

            variables.AddRange(variable.Value.GetAllVariables());
            variables.Add(variable);
            
            return variables;
        }

        var binaryExpression = value.AsT0!;
        
        variables.AddRange(binaryExpression.Left!.GetAllVariables());
        variables.AddRange(binaryExpression.Right!.GetAllVariables());

        return variables;
    }
    
    public static PossibleValue Clone(this PossibleValue value)
    {
        if (value.IsT2) return value.AsT2?.Clone() as string;

        if (value.IsT1)
        {
            var variable = value.AsT1;
            
            return new Variable(variable.Name, variable.Value.Clone());
        }

        var binaryExpression = value.AsT0!;
        
        return new BinaryExpressionVariable
        {
            Operation = (string)binaryExpression.Operation.Clone(),
            Left = binaryExpression.Left.Clone(),
            Right = binaryExpression.Right.Clone()
        };
    }
}