using TungstenCompiler.Parser;

namespace TungstenCompiler.AST;

internal static class StatementToAst
{
    internal static AstNode Convert(ParseNode parseNode)
    {
        return parseNode switch
        {
            VariableDeclarationAndAssignmentStatementParseNode variableDeclarationAndAssignment =>
                ConvertVariableDeclarationAndAssignmentStatement(variableDeclarationAndAssignment),
            VariableDeclarationStatementParseNode variableDeclaration =>
                ConvertVariableDeclarationStatement(variableDeclaration),
            AssignmentStatementParseNode assignment => ConvertAssignmentStatement(assignment),
            FunctionCallStatementParseNode functionCall => ConvertFunctionCallStatement(functionCall),

            _ => throw new Exception($"{parseNode.GetType().Name} is not a statement.")
        };
    }

    private static VariableDeclarationStatementAstNode
        ConvertVariableDeclarationStatement(VariableDeclarationStatementParseNode parseNode)
    {
        return new VariableDeclarationStatementAstNode(
            parseNode.Declaration.Identifier
        );
    }

    private static VariableDeclarationAndAssignmentStatementAstNode
        ConvertVariableDeclarationAndAssignmentStatement(VariableDeclarationAndAssignmentStatementParseNode parseNode)
    {
        return new VariableDeclarationAndAssignmentStatementAstNode(
            parseNode.Declaration.Identifier,
            parseNode.Declaration.Const,
            ExpressionToAst.Convert(parseNode.Value)
        );
    }

    private static AssignmentStatementAstNode ConvertAssignmentStatement(AssignmentStatementParseNode parseNode)
    {
        return new AssignmentStatementAstNode(
            parseNode.Identifier,
            ExpressionToAst.Convert(parseNode.Value)
        );
    }

    private static FunctionCallStatementAstNode ConvertFunctionCallStatement(FunctionCallStatementParseNode parseNode)
    {
        return new FunctionCallStatementAstNode(
            ExpressionToAst.ConvertFunctionCall(parseNode.FunctionCall)
        );
    }
}
