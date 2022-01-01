using Tungsten.Compiler.Parser.Expression;

namespace Tungsten.Compiler.Tests.Parser.Expression;

[TestClass]
public class AdditionParserTests : ParserTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(AdditionParser.Parse, ExpressionToAst.Convert, code, expected);
    }

    [TestMethod]
    public void IntLiteral()
    {
        TestParse("2", new IntAstNode(2));
    }

    [TestMethod]
    public void OneAddition()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        TestParse("1+2", expected);
    }

    [TestMethod]
    public void OneSubtraction()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Subtraction,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        TestParse("1-2", expected);
    }

    [TestMethod]
    public void TwoAdditions()
    {
        var addition12 = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            addition12,
            new IntAstNode(3)
        );

        TestParse("1+2+3", expected);
    }

    [TestMethod]
    public void AddThenSubtract()
    {
        var addition12 = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Subtraction,
            addition12,
            new IntAstNode(3)
        );

        TestParse("1+2-3", expected);
    }

    [TestMethod]
    public void MultiplyThenSubtract()
    {
        var multiply12 = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Subtraction,
            multiply12,
            new IntAstNode(3)
        );

        TestParse("1*2-3", expected);
    }

    [TestMethod]
    public void AddThenDivide()
    {
        var divide12 = new BinaryOperationAstNode(
            BinaryOperator.Division,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Addition,
            new IntAstNode(0),
            divide12
        );

        TestParse("0+1/2", expected);
    }
}
