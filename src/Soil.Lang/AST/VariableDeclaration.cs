namespace Soil.Lang.AST;

public record VariableDeclaration(
    NameIdentifier Name,
    TypeIdentifier? TypeName,
    Expression Initializer) : Declaration
{
    public VariableDeclaration(NameIdentifier name, Expression initializer)
        : this(name, null, initializer)
    {
    }
}