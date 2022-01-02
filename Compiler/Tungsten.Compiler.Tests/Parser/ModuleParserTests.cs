using Tungsten.Compiler.AST;
using Tungsten.Compiler.Parser;
using Tungsten.Compiler.Parser.Module;

namespace Tungsten.Compiler.Tests.Parser;

[TestClass]
public class ModuleParserTests : ParserTest
{
    private const string ModuleName = "TestModule";

    private static AstNode ConvertToAst(ParseNode parseNode)
    {
        Assert.IsInstanceOfType(parseNode, typeof(ModuleParseNode));
        return ModuleToAst.Convert((ModuleParseNode)parseNode, ModuleName);
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
        var code = @"
fun main(): void {
    print(""hello world"");
}
        ";

        var main = new FunctionDeclarationAstNode(
                "main",
                IsMain: true,
                new[]
                {
                Print(new StringAstNode("hello world"))
                }
            );

        var expected = new ModuleAstNode(
            ModuleName,
            new[]
            {
                main
            }
        );

        TestParse(code, expected);
    }

    [TestMethod]
    public void MultipleFunctions()
    {
        var code = @"
fun print3(): void {
    print(3);
}

fun main(): void {
    print3();
    print("".14159"");
}
        ";

        var print3 = new FunctionDeclarationAstNode(
            "print3",
            IsMain: false,
            Statements: new[]
            {
                Print(new IntAstNode(3))
            }
        );

        var main = new FunctionDeclarationAstNode(
            "main",
            IsMain: true,
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
            ModuleName,
            new[]
            {
                print3,
                main
            }
        );

        TestParse(code, expected);
    }

    [TestMethod]
    public void ConstVariable()
    {
        var code = @"
fun main(): void {
    const a: string = ""apple"";
    print(a);
}
        ";

        var main = new FunctionDeclarationAstNode(
            "main",
            IsMain: true,
            Statements: new AstNode[]
            {
                new VariableDeclarationAndAssignmentStatementAstNode(
                    "a",
                    Type: "string",
                    Const: true,
                    new StringAstNode("apple")
                ),
                Print(new VariableReferenceAstNode("a"))
            }
        );

        var expected = new ModuleAstNode(
            ModuleName,
            new[]
            {
                main
            }
        );

        TestParse(code, expected);
    }

    [TestMethod]
    public void LetVariable()
    {
        var code = @"
fun main(): void {
    let my_variable: int = 7;
    print(my_variable);

    my_variable = 3;
    print(my_variable);
}
        ";

        var main = new FunctionDeclarationAstNode(
            "main",
            IsMain: true,
            Statements: new AstNode[]
            {
                new VariableDeclarationAndAssignmentStatementAstNode(
                    "my_variable",
                    Type: "int",
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
            ModuleName,
            new[]
            {
                main
            }
        );

        TestParse(code, expected);
    }
}
