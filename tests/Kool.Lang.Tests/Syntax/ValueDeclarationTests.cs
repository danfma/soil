using FluentAssertions;
using Irony.Parsing;
using Kool.Lang.Syntax;
using Xunit;

namespace Kool.Lang.Tests.Syntax
{
    public class ValueDeclarationTests
    {
        private readonly KoolGrammarV1 _grammar = Grammars.KoolV1;

        [Fact]
        public void ParseIntValueDeclaration()
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

        [Fact]
        public void ParseStringValueDeclaration()
        {
            var text = @"let name = ""Daniel""";
            var result = ParserHelper.Parse(_grammar, text);

            result.Status.Should().Be(ParseTreeStatus.Parsed);

            var producedSyntaxNode = _grammar.Translate(result.Root);

            var expectedSyntaxNode = new CompilationUnit(
                new ValueDeclaration(
                    new NameIdentifier("name"),
                    new ConstantExpression(new StringValue("Daniel"))));

            producedSyntaxNode.Should().BeEquivalentTo(expectedSyntaxNode);
        }

        [Fact]
        public void ParseBooleanTrueValueDeclaration()
        {
            var text = @"let finished = true";
            var result = ParserHelper.Parse(_grammar, text);

            result.Status.Should().Be(ParseTreeStatus.Parsed);

            var producedSyntaxNode = _grammar.Translate(result.Root);

            var expectedSyntaxNode = new CompilationUnit(
                new ValueDeclaration(
                    new NameIdentifier("finished"),
                    new ConstantExpression(new BooleanValue(true))));

            producedSyntaxNode.Should().BeEquivalentTo(expectedSyntaxNode);
        }

        [Fact]
        public void ParseBooleanFalseValueDeclaration()
        {
            var text = @"let finished = false";
            var result = ParserHelper.Parse(_grammar, text);

            result.Status.Should().Be(ParseTreeStatus.Parsed);

            var producedSyntaxNode = _grammar.Translate(result.Root);

            var expectedSyntaxNode = new CompilationUnit(
                new ValueDeclaration(
                    new NameIdentifier("finished"),
                    new ConstantExpression(new BooleanValue(false))));

            producedSyntaxNode.Should().BeEquivalentTo(expectedSyntaxNode);
        }
    }
}