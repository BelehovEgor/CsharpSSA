using SSA.Common.Models;

namespace SSA.CfgParser;

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