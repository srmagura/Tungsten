using TFlat.Compiler.Parser;

namespace TFlat.Compiler.AST;

internal static class ModuleToAst
{
    internal static ModuleAstNode Convert(ModuleParseNode parseNode)
    {
        var functionDeclarations = parseNode.FunctionDeclarations
            .Select(ConvertFunctionDeclaration)
            .ToArray();

        return new ModuleAstNode(functionDeclarations);
    }

    private static FunctionDeclarationAstNode ConvertFunctionDeclaration(FunctionDeclarationParseNode parseNode)
    {
        var statements = parseNode.Statements
            .Select(StatementToAst.Convert)
            .ToArray();

        return new FunctionDeclarationAstNode(parseNode.Name, parseNode.Exported, statements);
    }
}
