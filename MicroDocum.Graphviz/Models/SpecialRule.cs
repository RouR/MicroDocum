using System;
using MicroDocum.Analyzers.Interfaces;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Graphviz.Models
{
    public sealed class SpecialRule<TK, TE>
    {
        private readonly Func<IChain<TK, LinkInfo<TE>>, string> _rule;

        public SpecialRule(Func<IChain<TK, LinkInfo<TE>>, string> rule)
        {
            _rule = rule;
        }

        public string Process(IChain<TK, LinkInfo<TE>> map)
        {
            return _rule(map);
        }
    }
}