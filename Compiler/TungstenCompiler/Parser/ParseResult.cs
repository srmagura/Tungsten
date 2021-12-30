namespace TFlat.Compiler.Parser;

internal record ParseResult<T>
    where T : ParseNode
{
    public ParseResult(T node, int consumedTokens)
    {
        Node = node;
        ConsumedTokens = consumedTokens;
    }

    public T Node { get; private init; }
    public int ConsumedTokens { get; private init; }
}
