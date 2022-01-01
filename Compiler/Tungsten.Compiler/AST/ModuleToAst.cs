using Tungsten.Compiler.Parser;

namespace Tungsten.Compiler.AST;

internal static class ModuleToAst
{
    internal static ModuleAstNode Convert(ModuleParseNode parseNode, string name)
    {
        var functionDeclarations = parseNode.FunctionDeclarations
            .Select(ConvertFunctionDeclaration)
            .ToArray();

        return new ModuleAstNode(name, functionDeclarations);
    }

    private static FunctionDeclarationAstNode ConvertFunctionDeclaration(FunctionDeclarationParseNode parseNode)
    {
        var statements = parseNode.Statements
            .Select(StatementToAst.Convert)
            .ToArray();

        // TODO check function arguments and return type
        var isMain = parseNode.Name == "main";

        return new FunctionDeclarationAstNode(parseNode.Name, isMain, statements);
    }
}
