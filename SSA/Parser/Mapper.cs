using SSA.Nodes.Models;

namespace SSA.Parser;

public static class Mapper
{
    public static PossibleValue Map(this Variable variable)
    {
        return variable;
    }
    
    public static PossibleValue Map(this string? variable)
    {
        return variable;
    }
}