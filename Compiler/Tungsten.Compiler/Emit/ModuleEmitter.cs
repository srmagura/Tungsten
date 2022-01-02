using Mono.Cecil;
using Mono.Cecil.Cil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class ModuleEmitter
{
    internal static (TypeDefinition Class, MethodDefinition? EntryPoint) Emit(
        ModuleAstNode ast,
        ModuleDefinition module,
        string rootNamespace
    )
    {
        var @class = new TypeDefinition(
            rootNamespace,
            ast.Name,
            TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
            module.ImportReference(typeof(Object))
        );

        MethodDefinition? mainMethod = null;

        foreach (var functionDeclaration in ast.FunctionDeclarations)
        {
            var method = Emit(functionDeclaration, module, @class);
            @class.Methods.Add(method);

            if (functionDeclaration.IsMain)
                mainMethod = method;
        }

        return (@class, mainMethod);
    }

    private static MethodDefinition Emit(FunctionDeclarationAstNode ast, ModuleDefinition module, TypeDefinition @class)
    {
        var method = new MethodDefinition(
            ast.Name,
            MethodAttributes.Static | MethodAttributes.Assembly | MethodAttributes.HideBySig,
            module.ImportReference(typeof(void))
        );

        var il = method.Body.GetILProcessor();

        foreach (var statement in ast.Statements)
        {
            StatementEmitter.Emit(statement, module, @class, il);
        }

        il.Emit(OpCodes.Ret);

        return method;
    }
}
