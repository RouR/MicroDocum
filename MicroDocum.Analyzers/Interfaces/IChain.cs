using System.Collections.Generic;

namespace MicroDocum.Analyzers.Interfaces
{
    public interface IChain<TN, TE>
    {
        IList<IChainNode<TN>> Nodes { get; }
         IList<IEdgeInfo<TE>> Edges { get; }
        void AddNode(IChainNode<TN> node);
        void AddDirectedEdge(IChainNode<TN> from, IChainNode<TN> to, TE data);
        IList<IChainNode<TN>> GetSingles();
        IList<IChainNode<TN>> GetHeads();
        IList<IChainNode<TN>> GetLeafs();
        IList<IChainNode<TN>> SearchLeafs(IChainNode<TN> head);
        IList<IChainNode<TN>> SearchHeads(IChainNode<TN> leaf);
        IList<IChain<TN, TE>> SplitChains();
        IChainNode<TN> this[string id] { get; }
    }
}
