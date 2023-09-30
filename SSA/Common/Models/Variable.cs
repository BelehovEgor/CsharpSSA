namespace SSA.Common.Models;

public record Variable(string Name, PossibleValue Value)
{
    protected const int DefaultVersion = -1;

    public int Version { get; set; } = DefaultVersion;
    
    public override string ToString()
    {
        var value = Value.Value?.ToString();
        
        if (Version == DefaultVersion)
        {
            return value is null
                ? Name
                : $"{Name} \u2190 {value}";
        }

        return value is null
            ? $"{Name}_{Version}"
            : $"{Name}_{Version} \u2190 {value}";
    }

    public virtual bool HasValue()
    {
        return Value is not {IsT2: true, AsT2: null};
    }
}