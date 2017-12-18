using System;
using System.Collections.Generic;
using MicroDocum.Analyzers.Interfaces;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Graphviz.Models
{
    public sealed class NodeRule<TK, TE>
    {
        private readonly Action<Node<TK>, IList<NodeRule<TK, TE>>, IChain<TK, LinkInfo<TE>>> _rule;

        public NodeRule(Action<Node<TK>, IList<NodeRule<TK, TE>>, IChain<TK, LinkInfo<TE>>> rule)
        {
            _rule = rule;
        }
       
        public void Process(Node<TK> node, IList<NodeRule<TK, TE>> nodeRules, IChain<TK, LinkInfo<TE>> map)
        {
            _rule(node, nodeRules, map);
        }
    }
}