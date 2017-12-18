using System;
using System.Collections.Generic;
using MicroDocum.Analyzers.Interfaces;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Graphviz.Models
{
    public sealed class LinkRule<T>
    {
        private readonly Action<IEdgeInfo<T>, Dictionary<string, string>, IChain<MessageInfo, T>> _rule;

        public LinkRule(Action<IEdgeInfo<T>, Dictionary<string, string>, IChain<MessageInfo, T>> rule)
        {
            _rule = rule;
        }
      
        public void Process(IEdgeInfo<T> edge, Dictionary<string, string> attributes, IChain<MessageInfo, T> map)
        {
            _rule(edge, attributes, map);
        }
        
    }
}
