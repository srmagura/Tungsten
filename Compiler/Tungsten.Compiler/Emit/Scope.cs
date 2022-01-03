namespace Tungsten.Compiler.Emit;

internal record Variable(string Identifier, WType Type, ushort Index);

internal class Scope
{
    private readonly Dictionary<string, Variable> Variables = new();

    private ushort Index = 0;

    internal ushort Define(string identifier, WType type)
    {
        Variables[identifier] = new Variable(identifier, type, Index);
        return Index++;
    }

    internal Variable GetOrThrow(string identifier)
    {
        if (!Variables.TryGetValue(identifier, out var variable))
            throw new Exception($"Variable not defined: {identifier}.");

        return variable;
    }
}
