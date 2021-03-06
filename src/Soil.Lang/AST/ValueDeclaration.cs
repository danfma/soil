namespace Soil.Lang.AST;

public record ValueDeclaration(
    NameIdentifier Name,
    TypeIdentifier? TypeName,
    Expression Initializer) : Declaration
{
    public ValueDeclaration(NameIdentifier name, Expression initializer)
        : this(name, null, initializer)
    {
    }
}