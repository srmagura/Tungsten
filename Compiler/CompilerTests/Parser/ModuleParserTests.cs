using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser;
using TFlat.Compiler.Parser.Module;

namespace CompilerTests.Parser;

[TestClass]
public class ModuleParserTests : ParserTest
{
    private static AstNode ConvertToAst(ParseNode parseNode)
    {
        Assert.IsInstanceOfType(parseNode, typeof(ModuleParseNode));
        return ModuleToAst.Convert((ModuleParseNode)parseNode);
    }

    private static void TestParse(string code, AstNode expected)
    {
        TestParseCore(ModuleParser.ParseModule, ConvertToAst, code, expected);
    }

    private static FunctionCallStatementAstNode Print(AstNode argument)
    {
        return new FunctionCallStatementAstNode(
            new FunctionCallAstNode(
                "print",
                new[] { argument }
            )
        );
    }

    [TestMethod]
    public void HelloWorld()
    {
        var main = new FunctionDeclarationAstNode(
            "main",
            Exported: false,
            new[]
            {
                Print(new StringAstNode("hello world"))
            }
        );

        var expected = new ModuleAstNode(
            new[]
            {
                main
            }
        );

        TestParse(CodeFixtures.HelloWorld, expected);
    }

    [TestMethod]
    public void MultipleFunctions()
    {
        var print3 = new FunctionDeclarationAstNode(
            "print3",
            Exported: false,
            Statements: new[]
            {
                Print(new IntAstNode(3))
            }
        );

        var main = new FunctionDeclarationAstNode(
            "main",
            Exported: false,
            Statements: new[]
            {
                new FunctionCallStatementAstNode(
                    new FunctionCallAstNode(
                        "print3",
                        Array.Empty<AstNode>()
                    )
                ),
                Print(new StringAstNode(".14159"))
            }
        );

        var expected = new ModuleAstNode(
            new[]
            {
                print3,
                main
            }
        );

        TestParse(CodeFixtures.MultipleFunctions, expected);
    }

    [TestMethod]
    public void ConstVariable()
    {
        var main = new FunctionDeclarationAstNode(
            "main",
            Exported: false,
            Statements: new AstNode[]
            {
                new VariableDeclarationAndAssignmentStatementAstNode(
                    "a",
                    Const: true,
                    new StringAstNode("apple")
                ),
                Print(new VariableReferenceAstNode("a"))
            }
        );

        var expected = new ModuleAstNode(
            new[]
            {
                main
            }
        );

        TestParse(CodeFixtures.ConstVariable, expected);
    }

    [TestMethod]
    public void LetVariable()
    {
        var main = new FunctionDeclarationAstNode(
            "main",
            Exported: false,
            Statements: new AstNode[]
            {
                new VariableDeclarationAndAssignmentStatementAstNode(
                    "my_variable",
                    Const: false,
                    new IntAstNode(7)
                ),
                Print(new VariableReferenceAstNode("my_variable")),
                new AssignmentStatementAstNode(
                    "my_variable",
                    new IntAstNode(3)
                ),
                Print(new VariableReferenceAstNode("my_variable")),
            }
        );

        var expected = new ModuleAstNode(
            new[]
            {
                main
            }
        );

        TestParse(CodeFixtures.LetVariable, expected);
    }
}
