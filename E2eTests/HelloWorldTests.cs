using System;
using System.IO;

namespace E2eTests;

[TestClass]
public class HelloWorldTests : E2eTest
{
    [TestMethod]
    public async Task HelloWorld()
    {
        var output = await BuildAndRunAsync("HelloWorld");
        Assert.AreEqual("hello world", output);
    }
}
