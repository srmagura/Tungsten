namespace TFlat.Shared;

public enum AstNodeType
{
    // Literals
    Bool,
    Int,
    String,

    // Expressions
    VariableReference,
    FunctionCall,
    UnaryOperation,
    BinaryOperation,

    // Statements
    VariableDeclarationStatement,
    VariableDeclarationAndAssignmentStatement,
    AssignmentStatement,
    FunctionCallStatement,

    // Top-level
    FunctionDeclaration,
    Module
}

public abstract record AstNode(AstNodeType Type);

// Literals

public record BoolAstNode(bool Value)
    : AstNode(AstNodeType.Bool);

public record IntAstNode(int Value)
    : AstNode(AstNodeType.Int);

public record StringAstNode(string Value)
    : AstNode(AstNodeType.String);

// Expressions

public record VariableReferenceAstNode(string Identifier)
    : AstNode(AstNodeType.VariableReference);

public record FunctionCallAstNode(string Function, AstNode[] Arguments)
    : AstNode(AstNodeType.FunctionCall);

public record UnaryOperationAstNode(UnaryOperator Operator, AstNode Operand)
    : AstNode(AstNodeType.UnaryOperation);

public record BinaryOperationAstNode(BinaryOperator Operator, AstNode Operand0, AstNode Operand1)
    : AstNode(AstNodeType.BinaryOperation);

// Statements

public record VariableDeclarationStatementAstNode(string Identifier)
    : AstNode(AstNodeType.VariableDeclarationStatement);

public record VariableDeclarationAndAssignmentStatementAstNode(string Identifier, bool Const, AstNode Value)
    : AstNode(AstNodeType.VariableDeclarationAndAssignmentStatement);

public record AssignmentStatementAstNode(string Identifier, AstNode Value)
    : AstNode(AstNodeType.AssignmentStatement);

public record FunctionCallStatementAstNode(FunctionCallAstNode FunctionCall)
    : AstNode(AstNodeType.FunctionCallStatement);

// Top-level

public record FunctionDeclarationAstNode(string Name, bool Exported, AstNode[] Statements)
    : AstNode(AstNodeType.FunctionDeclaration);

public record ModuleAstNode(FunctionDeclarationAstNode[] FunctionDeclarations)
    : AstNode(AstNodeType.Module);

public static class AstNodeTypeMap
{
    public static readonly Dictionary<AstNodeType, Type> Map = new()
    {
        // Literals
        [AstNodeType.Bool] = typeof(BoolAstNode),
        [AstNodeType.Int] = typeof(IntAstNode),
        [AstNodeType.String] = typeof(StringAstNode),
        
        // Expressions
        [AstNodeType.VariableReference] = typeof(VariableReferenceAstNode),
        [AstNodeType.FunctionCall] = typeof(FunctionCallAstNode),
        [AstNodeType.UnaryOperation] = typeof(UnaryOperationAstNode),
        [AstNodeType.BinaryOperation] = typeof(BinaryOperationAstNode),

        // Statements
        [AstNodeType.VariableDeclarationStatement] = typeof(VariableDeclarationStatementAstNode),
        [AstNodeType.VariableDeclarationAndAssignmentStatement] = typeof(VariableDeclarationAndAssignmentStatementAstNode),
        [AstNodeType.AssignmentStatement] = typeof(AssignmentStatementAstNode),
        [AstNodeType.FunctionCallStatement] = typeof(FunctionCallStatementAstNode),

        // Top-level
        [AstNodeType.FunctionDeclaration] = typeof(FunctionDeclarationAstNode),
        [AstNodeType.Module] = typeof(ModuleAstNode)
    };
}
