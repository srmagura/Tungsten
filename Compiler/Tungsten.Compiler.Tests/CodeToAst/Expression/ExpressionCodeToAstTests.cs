using Tungsten.Compiler.Parser.Expression;
using Tungsten.Compiler.Tests.CodeToAst;

namespace Tungsten.Compiler.Tests.CodeToAst.Expression;

[TestClass]
public class ExpressionCodeToAstTests : CodeToAstTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(ExpressionParser.Parse, ExpressionToAst.Convert, code, expected);
    }

    private static void TestDoesNotParse(string code)
    {
        TestDoesNotParseCore(ExpressionParser.Parse, code);
    }

    [TestMethod]
    public void Empty()
    {
        TestDoesNotParse("");
    }

    [TestMethod]
    public void Literals()
    {
        TestParse("2", new IntAstNode(2));
        TestParse("\"foo\"", new StringAstNode("foo"));
        TestParse("false", new BoolAstNode(false));
        TestParse("true", new BoolAstNode(true));
    }

    [TestMethod]
    public void BasicMath()
    {
        var divide23 = new BinaryOperationAstNode(
            BinaryOperator.Division,
            new IntAstNode(2),
            new IntAstNode(3)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntAstNode(1),
            divide23
        );

        TestParse("1+2/3", expected);
    }

    [TestMethod]
    public void Parentheses()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntAstNode(1),
            new IntAstNode(7)
        );

        TestParse("((1 + (7)))", expected);
    }

    [TestMethod]
    public void NumericNegation()
    {
        var negate3 = new UnaryOperationAstNode(
            UnaryOperator.NumericNegation,
            new IntAstNode(3)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            negate3,
            new IntAstNode(1)
        );

        TestParse("-3+1", expected);
    }

    [TestMethod]
    public void IntegerDivisionAndExponentiation()
    {
        var exponent23 = new BinaryOperationAstNode(
            BinaryOperator.Exponentiation,
            new IntAstNode(2),
            new IntAstNode(3)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.IntegerDivision,
            new IntAstNode(17),
            exponent23
        );

        TestParse(@"17 \\ 2**3", expected);
    }

    [TestMethod]
    public void ExponentationComesBeforeNumericNegation()
    {
        var exponent12 = new BinaryOperationAstNode(
            BinaryOperator.Exponentiation,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        var expected = new UnaryOperationAstNode(
            UnaryOperator.NumericNegation,
            exponent12
        );

        TestParse(@"-1**2", expected);
    }

    [TestMethod]
    public void Modulus()
    {
        var modulus172 = new BinaryOperationAstNode(
            BinaryOperator.Modulus,
            new IntAstNode(17),
            new IntAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Subtraction,
            new IntAstNode(1),
            modulus172
        );

        TestParse("1 - 17 % 2", expected);
    }
}
