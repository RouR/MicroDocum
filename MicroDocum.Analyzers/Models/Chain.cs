using System;
using System.Collections.Generic;
using System.Linq;
using MicroDocum.Analyzers.Interfaces;

namespace MicroDocum.Analyzers.Models
{
    public class Chain<TN, TE> : IChain<TN, TE>
    {
        public Chain()
        {
            Nodes = new List<IChainNode<TN>>();
            Edges = new List<IEdgeInfo<TE>>();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public bool CheckArguments { get; set; } = true;

        public IChainNode<TN> this[string key]
        {
            get { return Nodes.SingleOrDefault(x => x.Id == key); }
        }

        public IList<IChainNode<TN>> Nodes { get; }
        public IList<IEdgeInfo<TE>> Edges { get; }

        public void AddNode(IChainNode<TN> node)
        {
            if (CheckArguments)
            {
                if (node == null)
                    throw new ArgumentNullException(nameof(node));

                if (Nodes.Any(x => x.Id == node.Id))
                    throw new DuplicateException(node.Id);
            }

            Nodes.Add(node);
        }

        public void AddDirectedEdge(IChainNode<TN> from, IChainNode<TN> to, TE data)
        {
            if (CheckArguments)
            {
                if (from == null)
                    throw new ArgumentNullException(nameof(from));

                if (to == null)
                    throw new ArgumentNullException(nameof(to));

                if (Nodes.All(x => x.Id != from.Id))
                    throw new UnknownNodeException(from.Id);

                if (Nodes.All(x => x.Id != to.Id))
                    throw new UnknownNodeException(to.Id);
            }

            if (from.Id == to.Id)
            {
                //skip infinity loop
                return;
            }

            var forwardNodeIds = new List<string>();
            var backwardNodeIds = new List<string>();
            var isLoop = false;

            forwardNodeIds.Add(to.Id);
            VisitNodes(to, node =>
            {
                forwardNodeIds.Add(node.Id);
                if (node.Id == from.Id)
                    isLoop = true;
            });

            if (isLoop)
            {
                //search for nodes in loop
                backwardNodeIds.Add(from.Id);
                VisitNodesBack(from, node => { backwardNodeIds.Add(node.Id); });

                var loopIds = forwardNodeIds.Union(backwardNodeIds)
                    .Where(x => forwardNodeIds.Contains(x) && backwardNodeIds.Contains(x))
                    .Distinct().ToList();


                //try to find entry - it should be node nearest to head 

                var heads = GetHeads();
                var deep = new Dictionary<string, int>();
                var multiInIds = new List<string>();

                foreach (var head in heads)
                {
                    var lvl = 0;
                    VisitNodes(head, node =>
                    {
                        lvl++;
                        if (loopIds.Contains(node.Id))
                            if (deep.ContainsKey(node.Id))
                            {
                                deep[node.Id] = Math.Min(deep[node.Id], lvl);
                                multiInIds.Add(node.Id);
                            }
                            else
                            {
                                deep.Add(node.Id, lvl);
                            }
                    });
                }

                if (multiInIds.Count == 1 && multiInIds[0] == from.Id)
                {
                    multiInIds.Add(to.Id);
                    if (deep.ContainsKey(to.Id))
                        deep[to.Id] = -1;
                    else
                        deep.Add(to.Id, -1);
                }

                if (!multiInIds.Any())
                {
                    multiInIds.Add(to.Id);
                    if (deep.ContainsKey(to.Id))
                        deep[to.Id] = -1;
                    else
                        deep.Add(to.Id, -1);
                }

                var minLvl = deep.Where(x => multiInIds.Contains(x.Key)).Select(x => x.Value).Min();
                var loopEntry = this[deep.Where(x => multiInIds.Contains(x.Key)).First(x => x.Value == minLvl).Key];

                var nodeToEntry = Edges.Any(x => loopIds.Contains(x.FromId) && x.ToId == loopEntry.Id)
                    ? Edges.Single(x => loopIds.Contains(x.FromId) && x.ToId == loopEntry.Id).FromId
                    : from.Id;

                this[nodeToEntry].IsLoop = true;
            }

            Edges.Add(new EdgeType<TE>(from.Id, to.Id, data));
        }

        public IList<IChainNode<TN>> GetSingles()
        {
            var ids = Nodes.Select(x => x.Id).Except(Edges.Select(x => x.FromId)).Except(Edges.Select(x => x.ToId))
                .Distinct().ToList();
            return Nodes.Where(x => ids.Contains(x.Id)).ToList();
        }

        public IList<IChainNode<TN>> GetHeads()
        {
            var loop = Nodes.Where(x => x.IsLoop).Select(x => x.Id).ToList();
            var ids = Edges.Select(x => x.FromId)
                .Except(Edges.Where(x => loop.Contains(x.FromId) == false).Select(x => x.ToId)).Distinct().ToList();
            return Nodes.Where(x => ids.Contains(x.Id) && loop.Contains(x.Id) == false).ToList();
        }

        public IList<IChainNode<TN>> GetLeafs()
        {
            var ids = Edges.Select(x => x.ToId).Except(Edges.Select(x => x.FromId)).Distinct().ToList();
            return Nodes.Where(x => ids.Contains(x.Id)).ToList();
        }

        public IList<IChainNode<TN>> SearchLeafs(IChainNode<TN> head)
        {
            var result = new List<IChainNode<TN>>();
            var allLeafs = GetLeafs();
            VisitNodes(head, node =>
            {
                //add only final nodes
                if (allLeafs.Any(x => x.Id == node.Id))
                    result.Add(node);
            });
            return result;
        }

        public IList<IChainNode<TN>> SearchHeads(IChainNode<TN> leaf)
        {
            var result = new List<IChainNode<TN>>();
            var allHeads = GetHeads();
            VisitNodesBack(leaf, node =>
            {
                //add only final nodes
                if (allHeads.Any(x => x.Id == node.Id))
                    result.Add(node);
            });
            return result;
        }

        private void VisitNodes(IChainNode<TN> head, Action<IChainNode<TN>> processNode)
        {
            short infinityLoopDetector = short.MinValue;
            var checkNodes = new[] {head.Id};
            while (checkNodes.Length != 0)
            {
                if(++infinityLoopDetector == short.MaxValue-1)
                    throw new Exception("Infinity loop");

                var newChildIds = new List<string>();
                foreach (var node in checkNodes)
                {
                    var linkedTo = Edges
                        .Where(x => x.FromId == node && Nodes.Single(s => s.Id == x.FromId).IsLoop == false)
                        .Select(x => x.ToId).ToList();
                    linkedTo = linkedTo.Select(x => Nodes.Single(n => n.Id == x))
                        .Select(x => x.Id).ToList();
                    newChildIds.AddRange(linkedTo);
                    foreach (var id in linkedTo)
                        processNode(this[id]);
                }

                checkNodes = newChildIds.Distinct().ToArray();
            }
        }

        private void VisitNodesBack(IChainNode<TN> leaf, Action<IChainNode<TN>> processNode)
        {
            var checkNodes = new[] {leaf.Id};
            while (checkNodes.Length != 0)
            {
                var newChildIds = new List<string>();
                foreach (var node in checkNodes)
                {
                    var linkedTo = Edges.Where(x => x.ToId == node && Nodes.Single(s => s.Id == x.ToId).IsLoop == false)
                        .Select(x => x.FromId).ToList();
                    linkedTo = linkedTo.Select(x => Nodes.Single(n => n.Id == x))
                        .Select(x => x.Id).ToList();
                    newChildIds.AddRange(linkedTo);
                    foreach (var id in linkedTo)
                        processNode(this[id]);
                }

                checkNodes = newChildIds.Distinct().ToArray();
            }
        }

        public IList<IChain<TN, TE>> SplitChains()
        {
            var result = new List<IChain<TN, TE>>();

            var allHeads = GetHeads();
            //var allLeafs = GetLeafs();
            var allSingles = GetSingles();

            var nodes = Nodes.ToList(); //create copy

            foreach (var node in allSingles)
            {
                var chain = new Chain<TN, TE> {CheckArguments = CheckArguments};
                chain.AddNode(node);
                result.Add(chain);
                nodes.Remove(node);
            }

            void AddNodeToChain(IChain<TN, TE> chain1, IChainNode<TN> chainNode)
            {
                chain1.AddNode(chainNode);
                foreach (var edge in Edges)
                {
                    if (edge.FromId == chainNode.Id && chain1.Nodes.Any(x => x.Id == edge.ToId))
                        chain1.AddDirectedEdge(chainNode, chain1.Nodes.Single(x => x.Id == edge.ToId), edge.Info);

                    if (edge.ToId == chainNode.Id && chain1.Nodes.Any(x => x.Id == edge.FromId))
                        chain1.AddDirectedEdge(chain1.Nodes.Single(x => x.Id == edge.FromId), chainNode, edge.Info);
                }
            }

            while (nodes.Any())
            {
                var checkedNode = nodes.First();
                nodes.Remove(checkedNode);

                var fromLinks = Edges.Where(x => x.ToId == checkedNode.Id).ToArray();
                var toLinks = Edges.Where(x => x.FromId == checkedNode.Id).ToArray();
                var nearestNodeIds = fromLinks.Select(x => x.FromId).Union(toLinks.Select(x => x.ToId))
                    .Distinct()
                    .ToArray();

                var existChains = result.Where(x => x.Nodes.Any(n => nearestNodeIds.Contains(n.Id))).ToArray();
                if (existChains.Length == 0)
                {
                    //create new chain
                    var chain = new Chain<TN, TE> {CheckArguments = CheckArguments};
                    result.Add(chain);

                    chain.AddNode(checkedNode);
                }
                else if (existChains.Length == 1)
                {
                    //add to chain
                    var chain = existChains.First();

                    AddNodeToChain(chain, checkedNode);
                }
                else
                {
                    //merge chains and add to chain
                    var chain = new Chain<TN, TE> {CheckArguments = CheckArguments};
                    result.Add(chain);

                    foreach (var oldChain in existChains)
                    {
                        foreach (var item in oldChain.Nodes)
                            chain.Nodes.Add(item);
                        foreach (var item in oldChain.Edges)
                            chain.Edges.Add(item);

                        result.Remove(oldChain);
                    }

                    AddNodeToChain(chain, checkedNode);
                }
            }


            return result;
        }
    }
}