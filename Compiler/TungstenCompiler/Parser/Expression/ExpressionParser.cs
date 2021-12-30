using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser.Expression;

internal static class ExpressionParser
{
    internal static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var result = AdditionParser.Parse(tokens, position);
        if (result == null) return null;

        return ParseResultUtil.Generic(result);
    }
}
