namespace Tungsten.Compiler.Emit;

internal record Variable(string Identifier, string Type);

internal class Scope
{
    internal readonly Dictionary<string, Variable> Variables = new();

    internal Variable GetOrThrow(string identifier)
    {
        if (!Variables.TryGetValue(identifier, out var variable))
            throw new Exception($"Variable not defined: {identifier}.");

        return variable;
    }
}
