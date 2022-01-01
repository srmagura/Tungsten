using Mono.Cecil;
using Mono.Cecil.Cil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class ModuleEmitter
{
    internal static (TypeDefinition Class, MethodDefinition? EntryPoint) Emit(
        ModuleAstNode ast,
        ModuleDefinition moduleDefinition,
        string rootNamespace
    )
    {
        var baseType = moduleDefinition.ImportReference(typeof(Object));

        var programClass = new TypeDefinition(
            rootNamespace,
            "Program",
            TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
            baseType
        );

        var @void = moduleDefinition.ImportReference(typeof(void));
        var mainTA = MethodAttributes.Static | MethodAttributes.Assembly | MethodAttributes.HideBySig;
        var mainMethod = new MethodDefinition("main", mainTA, @void);

        var il = mainMethod.Body.GetILProcessor();
        il.Emit(OpCodes.Ldstr, "hello world");

        var writeLineRef = moduleDefinition.ImportReference(
            typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) })
        );
        il.Emit(OpCodes.Call, writeLineRef);

        il.Emit(OpCodes.Ret);

        programClass.Methods.Add(mainMethod);

        return (programClass, mainMethod);
    }
}
