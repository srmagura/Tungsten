using System;
using System.IO;

namespace E2eTests;

[TestClass]
public class TheE2eTests : E2eTest
{
    [TestMethod]
    public async Task HelloWorld()
    {
        var output = await BuildAndRunAsync("HelloWorld");
        Assert.AreEqual("hello world", output);
    }

    [TestMethod]
    public async Task MultipleFunctions()
    {
        var output = await BuildAndRunAsync("MultipleFunctions");
        Assert.AreEqual("3\n.14159", output);
    }

    [TestMethod]
    public async Task ConstVariable()
    {
        var output = await BuildAndRunAsync("ConstVariable");
        Assert.AreEqual("apple", output);
    }
}
