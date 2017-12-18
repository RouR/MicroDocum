using MicroDocum.Analyzers.Interfaces;

namespace MicroDocum.Analyzers.Models
{
    public sealed class ChainNode<TK> : IChainNode<TK>
    {
        public string Id { get; }
        public TK Node { get; }

        public bool IsLoop { get; set; }

        public ChainNode(string id, TK value)
        {
            Id = id;
            Node = value;
        }
    }
}