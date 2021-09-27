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
            var lineComment = new CommentTerminal(nameof(Symbol.LineComment), "//", "\r", "\n", "\u2028", "\u2029");
            var blockComment = new CommentTerminal(nameof(Symbol.BlockComment), "/*", "*/");

            NonGrammarTerminals.Add(lineComment);
            NonGrammarTerminals.Add(blockComment);

            // terminals
            var letKeyword = ToTerm("let", nameof(Symbol.Let));
            var varKeyword = ToTerm("var", nameof(Symbol.Var));
            var assignOperator = ToTerm("=", nameof(Symbol.Assign));
            var trueValue = ToTerm("true", nameof(Symbol.True));
            var falseValue = ToTerm("false", nameof(Symbol.False));

            var nameIdentifier = TerminalFactory.CreateCSharpIdentifier(nameof(Symbol.NameIdentifier));
            var stringValue = new StringLiteral(nameof(Symbol.String), "\"");
            var integerValue = new NumberLiteral(nameof(Symbol.Int), NumberOptions.IntOnly);

            // non-terminals
            var compilationUnit = new NonTerminal(nameof(Symbol.CompilationUnit));
            var statement = new NonTerminal(nameof(Symbol.Statement));
            var declaration = new NonTerminal(nameof(Symbol.Declaration));
            var valueDeclaration = new NonTerminal(nameof(Symbol.ValueDeclaration));
            var variableDeclaration = new NonTerminal(nameof(Symbol.VariableDeclaration));
            var expression = new NonTerminal(nameof(Symbol.Expression));
            var constantExpression = new NonTerminal(nameof(Symbol.ConstantExpression));
            var booleanValue = new NonTerminal(nameof(Symbol.Boolean));

            // derivations
            compilationUnit.Rule = statement;
            statement.Rule = declaration;
            declaration.Rule = valueDeclaration | variableDeclaration;
            valueDeclaration.Rule = letKeyword + nameIdentifier + assignOperator + expression;
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
                nameof(Symbol.CompilationUnit) => new CompilationUnit(
                    node.ChildNodes.Select(Translate).Cast<Statement>().ToArray()),

                nameof(Symbol.Statement) => Translate(node.ChildNodes.First()),

                nameof(Symbol.Declaration) => Translate(node.ChildNodes.First()),

                nameof(Symbol.ValueDeclaration) => new ValueDeclaration(
                    (NameIdentifier)Translate(node.ChildNodes[1]),
                    (Expression)Translate(node.ChildNodes[3])),

                nameof(Symbol.VariableDeclaration) => new VariableDeclaration(
                    (NameIdentifier)Translate(node.ChildNodes[1]),
                    (Expression)Translate(node.ChildNodes[3])),

                nameof(Symbol.NameIdentifier) => new NameIdentifier(node.Token.ValueString),

                nameof(Symbol.Expression) => Translate(node.ChildNodes[0]),

                nameof(Symbol.ConstantExpression) => new ConstantExpression(
                    (Literal)Translate(node.ChildNodes[0])),

                nameof(Symbol.Int) => new IntValue((int)node.Token.Value),

                nameof(Symbol.String) => new StringValue(node.Token.ValueString),

                nameof(Symbol.Boolean) => new BooleanValue(node.ChildNodes[0].Token.ValueString == "true"),

                _ => throw new NotSupportedException(
                    $"The token is not supported by this language version: {node.Term}")
            };
        }

        private enum Symbol
        {
            LineComment,
            BlockComment,
            Let,
            Var,
            Assign,
            NameIdentifier,
            Int,
            CompilationUnit,
            Statement,
            Declaration,
            ValueDeclaration,
            VariableDeclaration,
            Expression,
            ConstantExpression,
            String,
            True,
            False,
            Boolean
        }
    }
}