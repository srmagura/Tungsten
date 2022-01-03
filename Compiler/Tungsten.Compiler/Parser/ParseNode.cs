namespace Tungsten.Compiler.Parser;

internal abstract record ParseNode();

internal record EmptyParseNode()
    : ParseNode();

// Terminals

internal record BoolLiteralParseNode(bool Value)
    : ParseNode();

internal record IntLiteralParseNode(int Value)
    : ParseNode();

internal record StringLiteralParseNode(string Value)
    : ParseNode();

internal record IdentifierExpressionParseNode(string Identifier)
    : ParseNode();

// Expressions

/*
 * 
 * The production rules for operations like addition and multiplication have to be 
 * transformed to remove left recursion. That's why they are split up into "pre"
 * and "post" nodes. See https://www.csd.uwo.ca/~mmorenom/CS447/Lectures/Syntax.html/node8.html.
 *
 * Example
 * -------
 *
 * BEFORE:
 * 
 *    Expression => Expression + Int | Int
 *    
 * AFTER:
 * 
 *    Expression => Int Expression'
 *    Expression' => + Int Expression' | empty
 */

internal record UnaryOperationParseNode(UnaryOperator Operator, ParseNode Operand)
    : ParseNode();

internal record PostBinaryOperationParseNode(BinaryOperator Operator, ParseNode Operand, ParseNode Post)
    : ParseNode();

internal record BinaryOperationParseNode(ParseNode Operand, ParseNode Post)
    : ParseNode();

internal record ArgumentListParseNode(ParseNode[] Arguments)
    : ParseNode();

internal record FunctionCallParseNode(string Function, ArgumentListParseNode ArgumentList)
    : ParseNode();


// Type annotations

internal record TypeParseNode(WType Type)
    : ParseNode();

// Statement parts

internal record VariableDeclarationParseNode(string Identifier, bool Const, TypeParseNode TypeAnnotation)
    : ParseNode();

// Statements

internal record FunctionCallStatementParseNode(FunctionCallParseNode FunctionCall)
    : ParseNode();

internal record VariableDeclarationStatementParseNode(VariableDeclarationParseNode Declaration)
    : ParseNode();

internal record VariableDeclarationAndAssignmentStatementParseNode(VariableDeclarationParseNode Declaration, ParseNode Value)
    : ParseNode();

internal record AssignmentStatementParseNode(string Identifier, ParseNode Value)
    : ParseNode();

// Top-level

internal record FunctionDeclarationParseNode(string Name, ParseNode[] Statements)
    : ParseNode();

internal record ModuleParseNode(FunctionDeclarationParseNode[] FunctionDeclarations)
    : ParseNode();
