namespace Kool.Lang.Syntax
{
    public abstract record SyntaxNode;

    public abstract record Identifier : SyntaxNode;

    public record NameIdentifier(string Name) : Identifier;

    public abstract record Expression : SyntaxNode;

    public abstract record Literal : SyntaxNode;

    public record IntValue(int Value) : Literal;

    public record StringValue(string Value) : Literal;

    public record BooleanValue(bool Value) : Literal;

    public record ConstantExpression(Literal Value) : Expression;

    public abstract record Statement : SyntaxNode;

    public abstract record Declaration : Statement;

    public record TypeIdentifier(string Name) : Identifier;

    public record ValueDeclaration(
        NameIdentifier Name,
        TypeIdentifier? TypeName,
        Expression Initializer) : Declaration
    {
        public ValueDeclaration(NameIdentifier name, Expression expression) 
            : this(name, null, expression)
        {
        }
    }

    public record VariableDeclaration(NameIdentifier Name, Expression Initializer) : Declaration;

    public record CompilationUnit(params Statement[] Statements) : SyntaxNode;
}