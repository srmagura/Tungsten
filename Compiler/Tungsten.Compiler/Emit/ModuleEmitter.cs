using Mono.Cecil;
using Mono.Cecil.Cil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class ModuleEmitter
{
    internal static (TypeDefinition Class, MethodDefinition? EntryPoint) Emit(
        ModuleAstNode ast,
        AssemblyContext context
    )
    {
        var @class = new TypeDefinition(
            context.RootNamespace,
            ast.Name,
            TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
            context.Module.ImportReference(typeof(Object))
        );

        var moduleContext = new ModuleContext(context, @class);
        MethodDefinition? mainMethod = null;

        foreach (var functionDeclaration in ast.FunctionDeclarations)
        {
            var method = Emit(functionDeclaration, moduleContext);
            @class.Methods.Add(method);

            if (functionDeclaration.IsMain)
                mainMethod = method;
        }

        return (@class, mainMethod);
    }

    private static MethodDefinition Emit(FunctionDeclarationAstNode ast, ModuleContext context)
    {
        var method = new MethodDefinition(
            ast.Name,
            MethodAttributes.Static | MethodAttributes.Assembly | MethodAttributes.HideBySig,
            context.AssemblyContext.Module.ImportReference(typeof(void))
        );

        var il = method.Body.GetILProcessor();
        var functionContext = new FunctionContext(context, new Scope(), il);

        foreach (var statement in ast.Statements)
        {
            StatementEmitter.Emit(statement, functionContext);
        }

        il.Emit(OpCodes.Ret);

        return method;
    }
}
