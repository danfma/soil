namespace Soil.Lang.AST;

public sealed record CompilationUnit(
    TokenList<Statement> Statements
) : SyntaxNode
{
    public CompilationUnit(params Statement[] statements)
        : this(new TokenList<Statement>(statements))
    {
    }

    public CompilationUnit(IEnumerable<Statement> statements)
        : this(new TokenList<Statement>(statements))
    {
    }
}