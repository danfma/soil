namespace Kool.Lang.Grammar

open System
open FParsec


type SyntaxNode =
    | Identifier of IdentifierNode
    | Expression of ExpressionNode
    | Statement of StatementNode

and IdentifierNode =
    | Name of string

and ExpressionNode =
    | Literal of LiteralNode

and LiteralNode =
    | Integer of int

and StatementNode =
    | VariableDeclaration of VariableDeclarationNode

and VariableDeclarationNode =
    | Variable of Name: IdentifierNode
    | Value of Name: IdentifierNode

and Program = Program of List<StatementNode>


module Grammar =
    let private whitespace = skipMany (satisfy Char.IsWhiteSpace)
    let private letKeyword = pstring "let"

    let private identifier = letter .>>. manyChars (letter <|> digit <|> pchar '_') |>> fun (x, y) -> IdentifierNode.Name (string x + y)
    
    // assignment
    let private letAssignment = letKeyword >>. whitespace >>. identifier |>> VariableDeclarationNode.Variable
    
    let program: Parser<VariableDeclarationNode, string> = letAssignment


module Parser =
    let tryParse text = 
        Grammar.program text
        
