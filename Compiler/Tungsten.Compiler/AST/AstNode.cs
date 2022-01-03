namespace Tungsten.Compiler.AST;

internal abstract record AstNode();

internal abstract record ExpressionAstNode : AstNode
{
    protected ExpressionAstNode(WType? type)
    {
        Type = type;
    }

    internal WType? Type { get; set; }
}

// Literals

internal record BoolAstNode(bool Value)
    : ExpressionAstNode(WType.Bool);

internal record IntAstNode(long Value)
    : ExpressionAstNode(WType.Int);

internal record StringAstNode(string Value)
    : ExpressionAstNode(WType.String);

// Expressions

internal record VariableReferenceAstNode(string Identifier)
    : ExpressionAstNode(type: null);

internal record FunctionCallAstNode(string Function, ExpressionAstNode[] Arguments)
    : ExpressionAstNode(WType.Void); // TODO

internal record UnaryOperationAstNode(UnaryOperator Operator, ExpressionAstNode Operand)
    : ExpressionAstNode(type: null);

internal record BinaryOperationAstNode(BinaryOperator Operator, ExpressionAstNode Operand0, ExpressionAstNode Operand1)
    : ExpressionAstNode(type: null);

// Statements

internal record VariableDeclarationStatementAstNode(string Identifier)
    : AstNode();

internal record VariableDeclarationAndAssignmentStatementAstNode(string Identifier, WType Type, bool Const, ExpressionAstNode Value)
    : AstNode();

internal record AssignmentStatementAstNode(string Identifier, ExpressionAstNode Value)
    : AstNode();

internal record FunctionCallStatementAstNode(FunctionCallAstNode FunctionCall)
    : AstNode();

// Module-level

internal record FunctionDeclarationAstNode(string Name, bool IsMain, AstNode[] Statements)
    : AstNode();

// Top-level

internal record ModuleAstNode(string Name, FunctionDeclarationAstNode[] FunctionDeclarations)
    : AstNode();

internal record AssemblyAstNode(string Name, ModuleAstNode Module)
    : AstNode();
