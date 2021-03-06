using Mono.Cecil.Cil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class ExpressionEmitter
{
    internal static void Emit(AstNode ast, FunctionContext context)
    {
        var il = context.IL;

        switch(ast)
        {
            case StringAstNode @string:
                {
                    il.Emit(OpCodes.Ldstr, @string.Value);
                    break;
                }
            case IntAstNode @int:
                {
                    il.Emit(OpCodes.Ldc_I8, @int.Value);
                    break;
                }
            case VariableReferenceAstNode variableReference:
                {
                    var variable = context.Scope.GetOrThrow(variableReference.Identifier);
                    il.Emit(OpCodes.Ldloc, variable.Index);
                    break;
                }
            case BinaryOperationAstNode binaryOperation:
                {
                    Emit(binaryOperation.Operand0, context);
                    Emit(binaryOperation.Operand1, context);
                    il.Emit(OpCodes.Add);
                    break;
                }
            default:
                throw new Exception($"Expression type not supported: {ast.GetType().Name}.");
        }
    }
}
