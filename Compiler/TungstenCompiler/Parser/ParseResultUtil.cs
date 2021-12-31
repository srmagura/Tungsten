namespace TungstenCompiler.Parser;

internal static class ParseResultUtil
{
    internal static ParseResult<ParseNode> Generic<T>(ParseResult<T> result)
        where T : ParseNode
    {
        return new ParseResult<ParseNode>(result.Node, result.ConsumedTokens);
    }

    internal static readonly ParseResult<ParseNode> Empty = new(new EmptyParseNode(), 0);
}
