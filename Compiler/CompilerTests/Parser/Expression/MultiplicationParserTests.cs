using TungstenCompiler.Parser.Expression;

namespace CompilerTests.Parser.Expression;

[TestClass]
public class MultiplicationParserTests : ParserTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(MultiplicationParser.Parse, ExpressionToAst.Convert, code, expected);
    }

    [TestMethod]
    public void IntLiteral()
    {
        TestParse("2", new IntAstNode(2));
    }

    [TestMethod]
    public void OneMultiplcation()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        TestParse("1*2", expected);
    }

    [TestMethod]
    public void OneDivision()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.Division,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        TestParse("1/2", expected);
    }

    [TestMethod]
    public void OneIntegerDivision()
    {
        var expected = new BinaryOperationAstNode(
            BinaryOperator.IntegerDivision,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        TestParse(@"1\\2", expected);
    }

    [TestMethod]
    public void TwoMultiplications()
    {
        var multiply12 = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            multiply12,
            new IntAstNode(3)
        );

        TestParse("1*2*3", expected);
    }

    [TestMethod]
    public void MultiplyThenDivideThenIntegerDivide()
    {
        var multiply12 = new BinaryOperationAstNode(
            BinaryOperator.Multiplication,
            new IntAstNode(1),
            new IntAstNode(2)
        );

        var divideBy3 = new BinaryOperationAstNode(
            BinaryOperator.Division,
            multiply12,
            new IntAstNode(3)
        );

        var expected = new BinaryOperationAstNode(
            BinaryOperator.IntegerDivision,
            divideBy3,
            new IntAstNode(4)
        );

        TestParse(@"1*2/3\\4", expected);
    }
}
