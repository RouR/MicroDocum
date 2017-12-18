using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
// ReSharper disable ClassNeverInstantiated.Global

namespace MicroDocum.Themes.Tests.Examples
{
    [LabelAlt("altLabel")]
    [Tags("tag1", "tag22")]
    [TagsAlt("alt")]
    [TTL(2)]
    [ServiceName("EndpointName")]
    public class ClassNoCompileError: IProduce<StructNoCompileError>, IProduceOnce<IInterfaceNoCompileError>
    {
    }
}
