using TungstenCompiler.Lexer;
using TungstenCompiler.Parser.Expression;

namespace TungstenCompiler.Parser.Module;

internal static class StatementParser
{
    public static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var variableDeclarationStatement = ParseVariableDeclarationStatement(tokens, position);
        if (variableDeclarationStatement != null)
            return ParseResultUtil.Generic(variableDeclarationStatement);

        var variableDeclarationAndAssignmentStatement = ParseVariableDeclarationAndAssignmentStatement(tokens, position);
        if (variableDeclarationAndAssignmentStatement != null)
            return ParseResultUtil.Generic(variableDeclarationAndAssignmentStatement);

        var assignmentStatement = ParseAssignmentStatement(tokens, position);
        if (assignmentStatement != null)
            return ParseResultUtil.Generic(assignmentStatement);

        var functionCallStatement = ParseFunctionCallStatement(tokens, position);
        if (functionCallStatement != null)
            return ParseResultUtil.Generic(functionCallStatement);

        return null;
    }

    private static ParseResult<VariableDeclarationStatementParseNode>?
        ParseVariableDeclarationStatement(Token[] tokens, int position)
    {
        var i = position;

        var variableDeclarationResult = ParseVariableDeclaration(tokens, i);
        if (variableDeclarationResult == null) return null;
        i += variableDeclarationResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.Semicolon) return null;
        i++;

        return new ParseResult<VariableDeclarationStatementParseNode>(
            new VariableDeclarationStatementParseNode(variableDeclarationResult.Node),
            i - position
        );
    }

    private static ParseResult<VariableDeclarationAndAssignmentStatementParseNode>?
        ParseVariableDeclarationAndAssignmentStatement(Token[] tokens, int position)
    {
        var i = position;

        var variableDeclarationResult = ParseVariableDeclaration(tokens, i);
        if (variableDeclarationResult == null) return null;
        i += variableDeclarationResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.SingleEqual) return null;
        i++;

        var valueResult = ExpressionParser.Parse(tokens, i);
        if (valueResult == null) return null;
        i += valueResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.Semicolon) return null;
        i++;

        return new ParseResult<VariableDeclarationAndAssignmentStatementParseNode>(
            new VariableDeclarationAndAssignmentStatementParseNode(
                variableDeclarationResult.Node,
                valueResult.Node
            ),
            i - position
        );
    }

    private static ParseResult<AssignmentStatementParseNode>?
        ParseAssignmentStatement(Token[] tokens, int position)
    {
        var i = position;

        if (tokens[i].Type != TokenType.Identifier) return null;
        var identifier = tokens[i].Value;
        i++;

        if (tokens[i].Type != TokenType.SingleEqual) return null;
        i++;

        var valueResult = ExpressionParser.Parse(tokens, i);
        if (valueResult == null) return null;
        i += valueResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.Semicolon) return null;
        i++;

        return new ParseResult<AssignmentStatementParseNode>(
            new AssignmentStatementParseNode(
                identifier,
                valueResult.Node
            ),
            i - position
        );
    }

    internal static ParseResult<VariableDeclarationParseNode>? ParseVariableDeclaration(Token[] tokens, int position)
    {
        var i = position;

        if (tokens[i].Type != TokenType.ConstKeyword && tokens[i].Type != TokenType.LetKeyword)
            return null;

        var isConst = tokens[i].Type == TokenType.ConstKeyword;
        i++;

        if (tokens[i].Type != TokenType.Identifier) return null;
        var identifier = tokens[i].Value;
        i++;

        if (tokens[i].Type != TokenType.Colon) return null;
        i++;

        // TODO optional type annotations
        var typeResult = TypeParser.Parse(tokens, i);
        if (typeResult == null) return null;
        i += typeResult.ConsumedTokens;

        return new ParseResult<VariableDeclarationParseNode>(
            new VariableDeclarationParseNode(
                identifier,
                isConst,
                typeResult.Node
            ),
            i - position
        );
    }

    private static ParseResult<FunctionCallStatementParseNode>?
        ParseFunctionCallStatement(Token[] tokens, int position)
    {
        var i = position;

        var functionCallResult = FunctionCallParser.Parse(tokens, i);
        if (functionCallResult == null) return null;
        i += functionCallResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.Semicolon) return null;
        i++;

        return new ParseResult<FunctionCallStatementParseNode>(
            new FunctionCallStatementParseNode(functionCallResult.Node),
            i - position
        );
    }
}
