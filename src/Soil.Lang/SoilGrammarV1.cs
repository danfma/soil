using Irony.Parsing;
using Soil.Lang.AST;

namespace Soil.Lang;

[Language("Soil", "1.0", "Soil programming language")]
public class SoilGrammarV1 : Grammar
{
    public SoilGrammarV1()
    {
        // comments
        var lineComment = new CommentTerminal(nameof(TokenKind.LineComment), "//", "\r", "\n", "\u2028", "\u2029");
        var blockComment = new CommentTerminal(nameof(TokenKind.BlockComment), "/*", "*/");

        NonGrammarTerminals.Add(lineComment);
        NonGrammarTerminals.Add(blockComment);

        // terminals
        var assignOperator = ToTerm("=", nameof(TokenKind.Assign));
        var colon = ToTerm(":", nameof(TokenKind.Colon));
        var trueValue = ToTerm("true", nameof(TokenKind.True));
        var falseValue = ToTerm("false", nameof(TokenKind.False));

        var letKeyword = ToTerm("let", nameof(TokenKind.Let));
        var varKeyword = ToTerm("var", nameof(TokenKind.Var));

        // operators
        var plusOperator = ToTerm("+");
        var minusOperator = ToTerm("-");
        var multiplyOperator = ToTerm("*");
        var divideOperator = ToTerm("/");
        var moduloOperator = ToTerm("%");

        // pre-defined types
        var charType = ToTerm("Char", nameof(TokenKind.Char));
        var int8Type = ToTerm("Int8", nameof(TokenKind.Int8));
        var int16Type = ToTerm("Int16", nameof(TokenKind.Int16));
        var int32Type = ToTerm("Int32", nameof(TokenKind.Int32));
        var int64Type = ToTerm("Int64", nameof(TokenKind.Int64));
        var intType = ToTerm("Int", nameof(TokenKind.Int));
        var floatType = ToTerm("Float", nameof(TokenKind.Float));
        var doubleType = ToTerm("Double", nameof(TokenKind.Double));
        var boolType = ToTerm("Bool", nameof(TokenKind.Bool));
        var stringType = ToTerm("String", nameof(TokenKind.String));

        var preDefinedTypes = new[]
        {
            charType,
            int8Type,
            int16Type,
            int32Type,
            int64Type,
            intType,
            floatType,
            doubleType,
            boolType,
            stringType
        };

        MarkReservedWords(
            new List<string> { "let", "var", "true", "false" }
                .Concat(preDefinedTypes.Select(t => t.Name))
                .ToArray());

        var nameIdentifier = TerminalFactory.CreateCSharpIdentifier(nameof(TokenKind.NameIdentifier));
        var stringValue = new StringLiteral(nameof(TokenKind.String), "\"");
        var integerValue = new NumberLiteral(nameof(TokenKind.Int), NumberOptions.IntOnly);

        // non-terminals
        var compilationUnit = new NonTerminal(nameof(TokenKind.CompilationUnit));
        var statement = new NonTerminal(nameof(TokenKind.Statement));
        var declaration = new NonTerminal(nameof(TokenKind.Declaration));
        var valueDeclaration = new NonTerminal(nameof(TokenKind.ValueDeclaration));
        var variableDeclaration = new NonTerminal(nameof(TokenKind.VariableDeclaration));
        var expression = new NonTerminal(nameof(TokenKind.Expression));
        var constantExpression = new NonTerminal(nameof(TokenKind.ConstantExpression));
        var binaryExpression = new NonTerminal(nameof(TokenKind.BinaryExpression));
        var booleanValue = new NonTerminal(nameof(TokenKind.Bool));
        var binaryOperator = new NonTerminal(nameof(TokenKind.BinaryOperator));
        var variableIdentifier = new NonTerminal(nameof(TokenKind.Variable));
        var typeIdentifier = new NonTerminal(nameof(TokenKind.TypeIdentifier));
        var typeNameIdentifier = new NonTerminal(nameof(TokenKind.TypeNameIdentifier));
        var predefinedType = new NonTerminal(nameof(TokenKind.PredefinedType));

        // derivations
        compilationUnit.Rule =
            statement;

        statement.Rule =
            declaration;

        declaration.Rule =
            valueDeclaration | variableDeclaration;

        predefinedType.Rule =
            charType
            | int8Type
            | int16Type
            | int32Type
            | int64Type
            | intType
            | floatType
            | doubleType
            | boolType
            | stringType;

        typeIdentifier.Rule =
            colon + predefinedType
            | colon + typeNameIdentifier;

        typeNameIdentifier.Rule =
            nameIdentifier;

        valueDeclaration.Rule =
            letKeyword + nameIdentifier + typeIdentifier + assignOperator + expression
            | letKeyword + nameIdentifier + assignOperator + expression;

        variableDeclaration.Rule =
            varKeyword + nameIdentifier + typeIdentifier + assignOperator + expression
            | varKeyword + nameIdentifier + assignOperator + expression;

        variableIdentifier.Rule = nameIdentifier;

        expression.Rule =
            constantExpression
            | variableIdentifier
            | binaryExpression;

        constantExpression.Rule =
            integerValue | stringValue | booleanValue;

        binaryExpression.Rule =
            expression + binaryOperator + expression;

        booleanValue.Rule =
            trueValue | falseValue;

        binaryOperator.Rule =
            plusOperator | minusOperator | multiplyOperator | divideOperator | moduloOperator;

        Root = compilationUnit;
    }

    public SyntaxNode Translate(ParseTreeNode node)
    {
        return node.Term.Name switch
        {
            nameof(TokenKind.CompilationUnit) =>
                new CompilationUnit(
                    node.ChildNodes.Select(Translate).Cast<Statement>()),

            nameof(TokenKind.Statement) =>
                Translate(node.ChildNodes.First()),

            nameof(TokenKind.Declaration) =>
                Translate(node.ChildNodes.First()),

            nameof(TokenKind.ValueDeclaration)
                when node.ChildNodes[2].Term.Name == nameof(TokenKind.TypeIdentifier) =>
                new ValueDeclaration(
                    (NameIdentifier)Translate(node.ChildNodes[1]),
                    (TypeIdentifier)Translate(node.ChildNodes[2]),
                    (Expression)Translate(node.ChildNodes[4])),

            nameof(TokenKind.ValueDeclaration) =>
                new ValueDeclaration(
                    (NameIdentifier)Translate(node.ChildNodes[1]),
                    (Expression)Translate(node.ChildNodes[3])),

            nameof(TokenKind.VariableDeclaration)
                when node.ChildNodes[2].Term.Name == nameof(TokenKind.TypeIdentifier) =>
                new VariableDeclaration(
                    (NameIdentifier)Translate(node.ChildNodes[1]),
                    (TypeIdentifier)Translate(node.ChildNodes[2]),
                    (Expression)Translate(node.ChildNodes[4])),

            nameof(TokenKind.VariableDeclaration) =>
                new VariableDeclaration(
                    (NameIdentifier)Translate(node.ChildNodes[1]),
                    (Expression)Translate(node.ChildNodes[3])),

            nameof(TokenKind.Variable) =>
                new VariableIdentifier(
                    (NameIdentifier)Translate(node.ChildNodes[0])),

            nameof(TokenKind.NameIdentifier) =>
                new NameIdentifier(node.Token.ValueString),

            nameof(TokenKind.Expression) =>
                Translate(node.ChildNodes[0]),

            nameof(TokenKind.ConstantExpression) =>
                new ConstantExpression(
                    (Literal)Translate(node.ChildNodes[0])),

            nameof(TokenKind.BinaryExpression) =>
                new BinaryExpression(
                    (Expression)Translate(node.ChildNodes[0]),
                    (BinaryOperator)Translate(node.ChildNodes[1]),
                    (Expression)Translate(node.ChildNodes[2])),
            
            nameof(TokenKind.BinaryOperator) =>
                BinaryOperator.Parse(node.ChildNodes[0].Term.Name),

            nameof(TokenKind.TypeIdentifier) =>
                new TypeIdentifier(node.ChildNodes[1].ChildNodes[0].Token.ValueString),

            nameof(TokenKind.Int) =>
                new IntValue((int)node.Token.Value),

            nameof(TokenKind.String) =>
                new StringValue(node.Token.ValueString),

            nameof(TokenKind.Bool) =>
                new BooleanValue(node.ChildNodes[0].Token.ValueString == "true"),

            _ => throw new NotSupportedException(
                $"The token is not supported by this language version: {node.Term}")
        };
    }

    private enum TokenKind
    {
        LineComment,
        BlockComment,
        Let,
        Var,
        Colon,
        Assign,
        NameIdentifier,
        CompilationUnit,
        Statement,
        Declaration,
        ValueDeclaration,
        VariableDeclaration,
        Expression,
        ConstantExpression,
        BinaryExpression,
        Variable,
        TypeIdentifier,
        TypeNameIdentifier,
        PredefinedType,

        #region pre-defined types

        Int,
        String,
        Bool,
        Char,
        Int8,
        Int16,
        Int32,
        Int64,
        Float,
        Double,

        #endregion

        True,
        False,
        BinaryOperator
    }
}