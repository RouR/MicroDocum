using System.Collections.Generic;
using MicroDocum.Analyzers.Interfaces;

namespace MicroDocum.Graphviz.Models
{
    public sealed class Node<T>
    {
        private readonly IChainNode<T> _node;

        public Node(IChainNode<T> node)
        {
            _node = node;
        }

        /// <summary>
        /// http://www.graphviz.org/content/attrs
        /// </summary>
        public IDictionary<string, string> Attributes { get; set; }
        public string Name => _node.Id.NormalizeGraphvizNames();
        
        public string Comments { get; set; }
        public IChainNode<T> Info => _node;
    }
}
