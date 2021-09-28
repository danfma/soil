namespace Kool.Lang.Syntax
{
    public record TypeIdentifier(string Name) : Identifier
    {
        public static TypeIdentifier Bool => new TypeIdentifier("Bool");
        public static TypeIdentifier Int => new TypeIdentifier("Int");
    }
}