using Irony.Parsing;

namespace Soil.Lang.Tests;

public class ParserHelper : Grammar
{
    public static ParseTree Parse(Grammar grammar, string text, string? fileName = null)
    {
        var parser = new Parser(grammar);

        return parser.Parse(text, fileName);
    }
}