using MicroDocum.Analyzers.Interfaces;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Graphviz.Interfaces
{
    public interface IGraphvizGenerator<T>
    {
        string Generate(IChain<MessageInfo, LinkInfo<T>> map);
    }
}