using System.Collections.Generic;
using MicroDocum.Analyzers.Interfaces;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz.Models;

namespace MicroDocum.Graphviz.Interfaces
{
    public interface IGraphvizTheme<T> : IAnalyzerTheme<T>
    {
        IList<LinkRule<LinkInfo<T>>> DefaultLinkRules();
        IList<NodeRule<MessageInfo, T>> DefaultNodeRules();
        IList<SpecialRule<MessageInfo, T>> SpecialRulesPreProcess();
        IList<SpecialRule<MessageInfo, T>> SpecialRulesPostProcess();
    }
}
