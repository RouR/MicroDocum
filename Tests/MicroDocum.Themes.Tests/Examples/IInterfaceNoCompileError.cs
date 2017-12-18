using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace MicroDocum.Themes.Tests.Examples
{
    [TTL(3)]
    [ServiceName("EndpointName")]
    public interface IInterfaceNoCompileError : IProduceWeak<ClassNoCompileError>
    {
    }
}