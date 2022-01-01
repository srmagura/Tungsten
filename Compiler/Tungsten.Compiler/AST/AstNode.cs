namespace Tungsten.Compiler.AST;

public abstract record AstNode();

// Literals

public record BoolAstNode(bool Value)
    : AstNode();

public record IntAstNode(int Value)
    : AstNode();

public record StringAstNode(string Value)
    : AstNode();

// Expressions

public record VariableReferenceAstNode(string Identifier)
    : AstNode();

public record FunctionCallAstNode(string Function, AstNode[] Arguments)
    : AstNode();

public record UnaryOperationAstNode(UnaryOperator Operator, AstNode Operand)
    : AstNode();

public record BinaryOperationAstNode(BinaryOperator Operator, AstNode Operand0, AstNode Operand1)
    : AstNode();

// Statements

public record VariableDeclarationStatementAstNode(string Identifier)
    : AstNode();

public record VariableDeclarationAndAssignmentStatementAstNode(string Identifier, bool Const, AstNode Value)
    : AstNode();

public record AssignmentStatementAstNode(string Identifier, AstNode Value)
    : AstNode();

public record FunctionCallStatementAstNode(FunctionCallAstNode FunctionCall)
    : AstNode();

// Top-level

public record FunctionDeclarationAstNode(string Name, bool Exported, AstNode[] Statements)
    : AstNode();

public record ModuleAstNode(FunctionDeclarationAstNode[] FunctionDeclarations)
    : AstNode();
