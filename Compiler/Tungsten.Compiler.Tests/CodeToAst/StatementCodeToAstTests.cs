using Tungsten.Compiler.Parser.Module;

namespace Tungsten.Compiler.Tests.CodeToAst;

[TestClass]
public class StatementCodeToAstTests : CodeToAstTest
{
    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(StatementParser.Parse, StatementToAst.Convert, code, expected);
    }

    [TestMethod]
    public void FunctionCallWithNoArguments()
    {
        var expected = new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "f",
                Array.Empty<ExpressionAstNode>()
            )
        );

        TestParse("f();", expected);
    }

    [TestMethod]
    public void PrintIntLiteral()
    {
        var expected = new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[]
                {
                    new IntAstNode(3)
                }
            )
        );

        TestParse("print(3);", expected);
    }

    [TestMethod]
    public void PrintStringLiteral()
    {
        var expected = new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[]
                {
                    new StringAstNode("hello world")
                }
            )
        );

        TestParse(@"print(""hello world"");", expected);
    }

    [TestMethod]
    public void VariableDeclaration()
    {
        var expected = new VariableDeclarationStatementAstNode("a");

        TestParse("let a: string;", expected);
    }

    [TestMethod]
    public void ConstVariable()
    {
        var expected = new VariableDeclarationAndAssignmentStatementAstNode(
            "a",
            Type: WType.String,
            Const: true,
            new StringAstNode("apple")
        );

        TestParse(@"const a: string = ""apple"";", expected);
    }

    [TestMethod]
    public void LetVariableWithAssignment()
    {
        var expected = new VariableDeclarationAndAssignmentStatementAstNode(
            "my_variable",
            Type: WType.Int,
            Const: false,
            new IntAstNode(7)
        );

        TestParse("let my_variable: int = 7;", expected);
    }

    [TestMethod]
    public void Assignment()
    {
        var expected = new AssignmentStatementAstNode(
            "my_variable",
            new IntAstNode(3)
        );

        TestParse("my_variable = 3;", expected);
    }

    [TestMethod]
    public void PrintVariable()
    {
        var expected = new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[]
                {
                    new VariableReferenceAstNode("a")
                }
            )
        );

        TestParse("print(a);", expected);
    }
}
