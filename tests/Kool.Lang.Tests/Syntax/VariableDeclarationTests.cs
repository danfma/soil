using FluentAssertions;
using Irony.Parsing;
using Kool.Lang.Syntax;
using Xunit;

namespace Kool.Lang.Tests.Syntax
{
    public class VariableDeclarationTests
    {
        private readonly KoolGrammarV1 _grammar = Grammars.KoolV1;

        [Fact]
        public void ParseIntVariableDeclaration()
        {
            var text = "var x = 10";
            var result = ParserHelper.Parse(_grammar, text);

            result.Status.Should().Be(ParseTreeStatus.Parsed);

            var producedSyntaxNode = _grammar.Translate(result.Root);

            var expectedSyntaxNode = new CompilationUnit(
                new VariableDeclaration(
                    new NameIdentifier("x"),
                    new ConstantExpression(new IntValue(10))));

            producedSyntaxNode.Should().Be(expectedSyntaxNode);
        }

        [Fact]
        public void ParseStringVariableDeclaration()
        {
            var text = @"var name = ""Daniel""";
            var result = ParserHelper.Parse(_grammar, text);

            result.Status.Should().Be(ParseTreeStatus.Parsed);

            var producedSyntaxNode = _grammar.Translate(result.Root);

            var expectedSyntaxNode = new CompilationUnit(
                new VariableDeclaration(
                    new NameIdentifier("name"),
                    new ConstantExpression(new StringValue("Daniel"))));

            producedSyntaxNode.Should().Be(expectedSyntaxNode);
        }

        [Fact]
        public void ParseBooleanTrueVariableDeclaration()
        {
            var text = @"var finished = true";
            var result = ParserHelper.Parse(_grammar, text);

            result.Status.Should().Be(ParseTreeStatus.Parsed);

            var producedSyntaxNode = _grammar.Translate(result.Root);

            var expectedSyntaxNode = new CompilationUnit(
                new VariableDeclaration(
                    new NameIdentifier("finished"),
                    new ConstantExpression(new BooleanValue(true))));

            producedSyntaxNode.Should().Be(expectedSyntaxNode);
        }

        [Fact]
        public void ParseBooleanFalseVariableDeclaration()
        {
            var text = @"var finished = false";
            var result = ParserHelper.Parse(_grammar, text);

            result.Status.Should().Be(ParseTreeStatus.Parsed);

            var producedSyntaxNode = _grammar.Translate(result.Root);

            var expectedSyntaxNode = new CompilationUnit(
                new VariableDeclaration(
                    new NameIdentifier("finished"),
                    new ConstantExpression(new BooleanValue(false))));

            producedSyntaxNode.Should().Be(expectedSyntaxNode);
        }
    }
}