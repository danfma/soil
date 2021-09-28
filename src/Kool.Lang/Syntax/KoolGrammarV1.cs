using System;
using System.Linq;
using Irony.Parsing;

namespace Kool.Lang.Syntax
{
    [Language("Kool", "1.0", "Kool programming language")]
    public class KoolGrammarV1 : Grammar
    {
        public KoolGrammarV1()
        {
            // comments
            var lineComment = new CommentTerminal(nameof(TokenKind.LineComment), "//", "\r", "\n", "\u2028", "\u2029");
            var blockComment = new CommentTerminal(nameof(TokenKind.BlockComment), "/*", "*/");

            NonGrammarTerminals.Add(lineComment);
            NonGrammarTerminals.Add(blockComment);

            // terminals
            var assignOperator = ToTerm("=", nameof(TokenKind.Assign));
            var colon = ToTerm(":", nameof(TokenKind.Colon));
            var letKeyword = ToTerm("let", nameof(TokenKind.Let));
            var varKeyword = ToTerm("var", nameof(TokenKind.Var));
            var trueValue = ToTerm("true", nameof(TokenKind.True));
            var falseValue = ToTerm("false", nameof(TokenKind.False));

            MarkReservedWords("let", "var", "true", "false");

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
            var booleanValue = new NonTerminal(nameof(TokenKind.Bool));
            var typeIdentifier = new NonTerminal(nameof(TokenKind.TypeIdentifier));
            var typeNameIdentifier = new NonTerminal(nameof(TokenKind.TypeNameIdentifier));

            // derivations
            compilationUnit.Rule = statement;
            statement.Rule = declaration;
            declaration.Rule = valueDeclaration | variableDeclaration;
            valueDeclaration.Rule =
                letKeyword + nameIdentifier + typeIdentifier + assignOperator + expression
                | letKeyword + nameIdentifier + assignOperator + expression;
            typeIdentifier.Rule = colon + typeNameIdentifier;
            typeNameIdentifier.Rule = nameIdentifier;
            variableDeclaration.Rule = varKeyword + nameIdentifier + assignOperator + expression;
            expression.Rule = constantExpression;
            constantExpression.Rule = integerValue | stringValue | booleanValue;
            booleanValue.Rule = trueValue | falseValue;

            Root = compilationUnit;
        }

        public SyntaxNode Translate(ParseTreeNode node)
        {
            return node.Term.Name switch
            {
                nameof(TokenKind.CompilationUnit) => new CompilationUnit(
                    node.ChildNodes.Select(Translate).Cast<Statement>().ToArray()),

                nameof(TokenKind.Statement) => Translate(node.ChildNodes.First()),

                nameof(TokenKind.Declaration) => Translate(node.ChildNodes.First()),

                nameof(TokenKind.ValueDeclaration)
                    when node.ChildNodes[2].Term.Name == nameof(TokenKind.TypeIdentifier) =>
                    new ValueDeclaration(
                        (NameIdentifier)Translate(node.ChildNodes[1]),
                        (TypeIdentifier)Translate(node.ChildNodes[2]),
                        (Expression)Translate(node.ChildNodes[4])),

                nameof(TokenKind.ValueDeclaration) => new ValueDeclaration(
                    (NameIdentifier)Translate(node.ChildNodes[1]),
                    (Expression)Translate(node.ChildNodes[3])),

                nameof(TokenKind.VariableDeclaration) => new VariableDeclaration(
                    (NameIdentifier)Translate(node.ChildNodes[1]),
                    (Expression)Translate(node.ChildNodes[3])),

                nameof(TokenKind.NameIdentifier) => new NameIdentifier(node.Token.ValueString),

                nameof(TokenKind.Expression) => Translate(node.ChildNodes[0]),

                nameof(TokenKind.ConstantExpression) => new ConstantExpression(
                    (Literal)Translate(node.ChildNodes[0])),

                nameof(TokenKind.TypeIdentifier) =>
                    new TypeIdentifier(node.ChildNodes[1].ChildNodes[0].Token.ValueString),

                nameof(TokenKind.Int) => new IntValue((int)node.Token.Value),

                nameof(TokenKind.String) => new StringValue(node.Token.ValueString),

                nameof(TokenKind.Bool) => new BooleanValue(node.ChildNodes[0].Token.ValueString == "true"),

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
            Assign,
            NameIdentifier,
            CompilationUnit,
            Statement,
            Declaration,
            ValueDeclaration,
            VariableDeclaration,
            Expression,
            ConstantExpression,
            Int,
            String,
            Bool,
            True,
            False,
            TypeIdentifier,
            Colon,
            TypeNameIdentifier
        }
    }
}