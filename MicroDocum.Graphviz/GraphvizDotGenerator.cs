using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroDocum.Analyzers.Interfaces;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz.Enums;
using MicroDocum.Graphviz.Interfaces;
using MicroDocum.Graphviz.Models;

namespace MicroDocum.Graphviz
{
    public sealed class GraphvizDotGenerator<T> : GraphvizGenerator, IGraphvizGenerator<T>
    {
        private readonly IGraphvizTheme<T> _theme;

        public GraphvizDotGenerator(IGraphvizTheme<T> theme) : base(GraphRenderingEngine.Dot)
        {
            _theme = theme;
        }

        public string Generate(IChain<MessageInfo, LinkInfo<T>> map)
        {
            var result = new StringBuilder();
            result.AppendLine("digraph architecture {\n");
            result.AppendLine(GetAbout());
            result.AppendLine("\n");
            result.AppendLine(SpecialRulesPreProcess(map));

            result.AppendLine(GetNodesDescription(map));

            foreach (var chain in map.SplitChains())
            {
                result.AppendLine("\n\n");
                result.AppendLine(GetSubgraph(chain));
            }

            result.AppendLine(SpecialRulesPostProcess(map));
            result.AppendLine(GetFinalData(map));

            return result.ToString().Replace("\n", Environment.NewLine);
        }

        private string CheckAndWrapQuotes(string text)
        {
            if (null == text)
                return $"\"\"";

            if (text.StartsWith("<")) // It can be HTML-Like Labels https://graphviz.gitlab.io/_pages/doc/info/shapes.html#html
                return text;

            //else wrap with quotes
            return $"\"{text}\""; 
        }

        private string GetNodesDescription(IChain<MessageInfo, LinkInfo<T>> map)
        {
            var sb = new StringBuilder();
            foreach (var node in map.Nodes)
            {
                var processedNode = new Node<MessageInfo>(node)
                {
                    Attributes = new Dictionary<string, string>(4)
                };
                foreach (var nodeRule in _theme.DefaultNodeRules())
                {
                    nodeRule.Process(processedNode, _theme.DefaultNodeRules(), map);
                }

                if (!string.IsNullOrEmpty(processedNode.Comments))
                    sb.AppendLine($"/* {processedNode.Comments} */");

                var additional = string.Empty;
                if (processedNode.Attributes.Count > 0)
                    additional = $"[{string.Join(", ", processedNode.Attributes.Select(x => $"{x.Key.ToLower()}={CheckAndWrapQuotes(x.Value)}").ToArray())}]";
                
                sb.AppendLine($"{processedNode.Name}{additional};");
            }
            return sb.ToString();
        }

        private string GetSubgraph(IChain<MessageInfo, LinkInfo<T>> chain)
        {
            var sb = new StringBuilder();

            //First the name of the subgraphs are important, to be visually separated they must be prefixed with cluster_
            var subgraphName = ((_theme.BorderedChains() ? $"cluster_" : "") + Guid.NewGuid()).NormalizeGraphvizNames();
            sb.AppendLine($"subgraph {subgraphName} {{");

            foreach (var edge in chain.Edges)
            {

                sb.AppendLine();
              
                var attributes = new Dictionary<string, string>();
                
                if (!string.IsNullOrEmpty(edge.Info.Label))
                    attributes.Add(GraphvizAttribute.Label.ToString(), edge.Info.Label);
                
                foreach (var linkRule in _theme.DefaultLinkRules())
                {
                    linkRule.Process(edge, attributes, chain);
                }

                sb.Append($"{edge.FromId.NormalizeGraphvizNames()} -> {edge.ToId.NormalizeGraphvizNames()}");
                if (attributes.Count > 0)
                    sb.Append($"[{string.Join(", ", attributes.Select(x => $"{x.Key.ToLower()}={CheckAndWrapQuotes(x.Value)}").ToArray())}]");

                sb.Append(";");

            }
            sb.AppendLine();
            sb.AppendLine("}");

            return sb.ToString();
        }

        private string SpecialRulesPreProcess(IChain<MessageInfo, LinkInfo<T>> map)
        {
            var sb = new StringBuilder();

            foreach (var rule in _theme.SpecialRulesPreProcess())
            {
                sb.AppendLine(rule.Process(map));
            }

            return sb.ToString();
        }
        private string SpecialRulesPostProcess(IChain<MessageInfo, LinkInfo<T>> map)
        {
            var sb = new StringBuilder();

            foreach (var rule in _theme.SpecialRulesPostProcess())
            {
                sb.AppendLine(rule.Process(map));
            }

            return sb.ToString();
        } 
        
        // ReSharper disable once UnusedParameter.Local
        private string GetFinalData(IChain<MessageInfo, LinkInfo<T>> map)
        {
            return "}";
        }
    }
}