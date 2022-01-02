using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Tungsten.Compiler.Emit;

internal record AssemblyContext(ModuleDefinition Module, String RootNamespace);

internal record ModuleContext(AssemblyContext AssemblyContext, TypeDefinition Class);

internal record FunctionContext(ModuleContext ModuleContext, Scope Scope, ILProcessor IL);
