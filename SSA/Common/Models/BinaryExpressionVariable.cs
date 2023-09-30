namespace SSA.Common.Models;

public class BinaryExpressionVariable
{
    public required PossibleValue Left { get; init; }
    public required PossibleValue Right { get; init; }
    public required string Operation { get; init; }

    public override string ToString()
    {
        return $"{Left.Value} {Operation} {Right.Value}";
    }
}