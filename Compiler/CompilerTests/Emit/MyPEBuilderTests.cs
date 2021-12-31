using TungstenCompiler.Emit;

namespace CompilerTests.Emit;

[TestClass]
public class MyPEBuilderTests
{
    [TestMethod]
    public void WritePE()
    {
        using var dllStream = File.OpenWrite("TEST.dll");
        MyPEBuilder.Build(dllStream);
    }
}
