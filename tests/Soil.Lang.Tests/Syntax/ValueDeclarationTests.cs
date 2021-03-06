using FluentAssertions;
using Irony.Parsing;
using Soil.Lang.AST;
using Xunit;

namespace Soil.Lang.Tests.Syntax;

public class ValueDeclarationTests
{
    private readonly SoilGrammarV1 _grammar = Grammars.SoilV1;

    [Fact]
    public void ParseBooleanTrueValueDeclaration()
    {
        var text = @"val finished = true";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("finished"),
                new ConstantExpression(new BooleanValue(true))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }

    [Fact]
    public void ParseBooleanFalseValueDeclaration()
    {
        var text = @"val finished = false";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("finished"),
                new ConstantExpression(new BooleanValue(false))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }

    [Fact]
    public void ParseBooleanWithTypeValue()
    {
        var text = @"val finished: Bool = false";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("finished"),
                new TypeIdentifier("Bool"),
                new ConstantExpression(new BooleanValue(false))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }

    [Fact]
    public void ParseIntValueDeclaration()
    {
        var text = "val x = 10";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("x"),
                new ConstantExpression(new IntValue(10))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }

    [Fact]
    public void ParseIntValueWithTypeDeclaration()
    {
        var text = "val x: Int = 10";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("x"),
                new TypeIdentifier("Int"),
                new ConstantExpression(new IntValue(10))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }

    [Fact]
    public void ParseStringValueDeclaration()
    {
        var text = @"val name = ""John Doe""";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("name"),
                new ConstantExpression(new StringValue("John Doe"))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }

    [Fact]
    public void ParseStringValueWithTypeDeclaration()
    {
        var text = @"val name: String = ""John Doe""";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("name"),
                new TypeIdentifier("String"),
                new ConstantExpression(new StringValue("John Doe"))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }

    [Fact]
    public void ParseVariableReassignment()
    {
        var text = @"val finished: Bool = other";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("finished"),
                new TypeIdentifier("Bool"),
                new VariableIdentifier(new NameIdentifier("other"))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }

    [Fact]
    public void ParseExpressionAssignment()
    {
        var text = @"val finished: Int = 1 + 2 * otherValue";
        var result = ParserHelper.Parse(_grammar, text);

        result.Status.Should().Be(ParseTreeStatus.Parsed);

        var producedSyntaxNode = _grammar.Translate(result.Root);

        var expectedSyntaxNode = new CompilationUnit(
            new ValueDeclaration(
                new NameIdentifier("finished"),
                new TypeIdentifier("Int"),
                new BinaryExpression(
                    new ConstantExpression(new IntValue(1)),
                    BinaryOperator.Plus,
                    new BinaryExpression(
                        new ConstantExpression(new IntValue(2)),
                        BinaryOperator.Multiply,
                        new VariableIdentifier(new NameIdentifier("otherValue"))))));

        producedSyntaxNode.Should().Be(expectedSyntaxNode);
    }
}