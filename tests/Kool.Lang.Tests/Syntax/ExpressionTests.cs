using FluentAssertions;
using Irony.Parsing;
using Kool.Lang.Syntax;
using Xunit;

namespace Kool.Lang.Tests.Syntax
{
    public class ExpressionTests
    {
        private readonly KoolGrammarV1 _grammar = new();

        [Fact]
        public void ParseLetExpression()
        {
            var text = "let x = 10";
            var result = ParserHelper.Parse(_grammar, text);

            result.Status.Should().Be(ParseTreeStatus.Parsed);

            var producedSyntaxNode = _grammar.Translate(result.Root);

            var expectedSyntaxNode = new CompilationUnit(
                new ValueDeclaration(
                    new NameIdentifier("x"),
                    new ConstantExpression(new IntValue(10))));

            producedSyntaxNode.Should().BeEquivalentTo(expectedSyntaxNode);
        }
    }
}