namespace Soil.Lang.AST;

public sealed record BinaryOperator : SyntaxNode
{
    private static readonly Dictionary<string, BinaryOperator> OperatorBySymbol = new();
    public static readonly BinaryOperator Plus = new("+");
    public static readonly BinaryOperator Minus = new("-");
    public static readonly BinaryOperator Multiply = new("*");
    public static readonly BinaryOperator Divide = new("/");
    public static readonly BinaryOperator Modulo = new("%");

    private BinaryOperator(string symbol)
    {
        Symbol = symbol;
        OperatorBySymbol.Add(symbol, this);
    }

    public string Symbol { get; }

    public static BinaryOperator Parse(string symbol)
    {
        if (!OperatorBySymbol.TryGetValue(symbol, out var binaryOperator))
            throw new ArgumentException($"Unknown binary operator '{symbol}'");

        return binaryOperator;
    }
}