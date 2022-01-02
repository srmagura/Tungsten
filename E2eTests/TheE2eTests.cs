using System;
using System.IO;

namespace E2eTests;

[TestClass]
public class TheE2eTests : E2eTest
{
    [TestMethod]
    public async Task HelloWorld()
    {
        var output = await BuildAndRunAsync(nameof(HelloWorld));
        Assert.AreEqual("hello world", output);
    }

    [TestMethod]
    public async Task MultipleFunctions()
    {
        var output = await BuildAndRunAsync(nameof(MultipleFunctions));
        Assert.AreEqual("3\n.14159", output);
    }

    [TestMethod]
    public async Task ConstVariable()
    {
        var output = await BuildAndRunAsync(nameof(ConstVariable));
        Assert.AreEqual("apple", output);
    }

    [TestMethod]
    public async Task LetVariable()
    {
        var output = await BuildAndRunAsync(nameof(LetVariable));
        Assert.AreEqual("7\n3", output);
    }

    [TestMethod]
    public async Task MultipleVariables()
    {
        var output = await BuildAndRunAsync(nameof(MultipleVariables));
        Assert.AreEqual("99\napple", output);
    }
}
