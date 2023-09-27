namespace SSA.Nodes.Models;

public class ParenthesizedBinaryExpressionVariable : BinaryExpressionVariable
{
    public override string ToString()
    {
        return $"({Left.Value} {Operation} {Right.Value})";
    }
}