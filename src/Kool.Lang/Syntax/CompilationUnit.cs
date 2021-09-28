using System.Collections.Generic;

namespace Kool.Lang.Syntax
{
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
}