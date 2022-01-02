using System;
using System.IO;

namespace E2eTests;

[TestClass]
public class MultipleFunctionsTests : E2eTest
{
    [TestMethod]
    public async Task MultipleFunctions()
    {
        var output = await BuildAndRunAsync("MultipleFunctions");
        Assert.AreEqual("3\n.14159", output);
    }
}
