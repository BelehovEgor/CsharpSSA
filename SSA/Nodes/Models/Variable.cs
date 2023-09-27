using OneOf;

namespace SSA.Nodes.Models;

using PossibleValue = OneOf<BinaryExpressionVariable, Variable, string?>;

public record Variable(string Name, int Version, PossibleValue Value)
{
    public override string ToString()
    {
        if (Value.TryPickT2(out var str, out var binaryOrVariable))
        {
            return str is null
                ? $"{Name}"
                : $"{Name} \u2190 {str}";
        }

        if (binaryOrVariable.TryPickT1(out var value, out var variable))
        {
            return $"{Name} \u2190 {value}";
        }
        
        return $"{Name} \u2190 {variable}";
    }
}

public static class VariableFactory
{
    private static readonly Dictionary<string, int> NameVersions = new();

    public static Variable Create(string name, string? value = null)
    {
        int newVersion = 1;
        
        if (NameVersions.TryGetValue(name, out var currentVersion))
        {
            newVersion = currentVersion + 1;
        }

        NameVersions[name] = newVersion;

        return new Variable(name, newVersion, value);
    }
}