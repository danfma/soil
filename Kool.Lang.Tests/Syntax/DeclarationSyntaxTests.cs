using System;
using FluentAssertions;
using Xunit;
using Kool.Lang.Grammar;

namespace Kool.Lang.Tests.Syntax
{
    public class DeclarationSyntaxTests
    {
        [Fact]
        public void ReadonlyVarDeclaration()
        {
            var text = "let x = 10";
            var token = new Parser.tryParse(text);

            token.Should().Be(
                new LetDeclarationNode(
                    new IdentifierName("x"),
                    new ConstantValueExpression(
                        new IntegerLiteral(10)
                    )
                )
            );

            token.Print().Should().Be(text);
        }
    }
}
