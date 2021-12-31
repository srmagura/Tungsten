using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace TungstenCompiler.Emit;

internal static class MyPEBuilder
{
    internal static void Build(Stream outStream)
    {
        var headerBuilder = new PEHeaderBuilder();

        var metadataBuilder = new MetadataBuilder();
        var metadataRootBuilder = new MetadataRootBuilder(metadataBuilder);

        var ilStream = new BlobBuilder();
        var peBuilder = new ManagedPEBuilder(headerBuilder, metadataRootBuilder, ilStream);

        var peBlob = new BlobBuilder();
        peBuilder.Serialize(peBlob);

        peBlob.WriteContentTo(outStream);
    }
}
