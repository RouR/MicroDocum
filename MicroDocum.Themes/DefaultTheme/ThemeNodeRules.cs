using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz.Enums;
using MicroDocum.Graphviz.Models;

// ReSharper disable MemberCanBePrivate.Global

namespace MicroDocum.Themes.DefaultTheme
{
    public static class ThemeNodeRules
    {
      public static readonly NodeRule<MessageInfo, DefaultLinkStyle> SetComments =
            new NodeRule<MessageInfo, DefaultLinkStyle>((node, nodeRules, map) =>
            {
                node.Comments = node.Info.Node.Type.FullName;
            });

        public static readonly NodeRule<MessageInfo, DefaultLinkStyle> SetLabel =
            new NodeRule<MessageInfo, DefaultLinkStyle>((node, nodeRules, map) =>
            {
                //You can get data from DescriptionAttribute, but it require nuget package System.ComponentModel.Primitives which is not compatible with .Net 3.5
                node.Attributes[GraphvizAttribute.Label.ToString()] = node.Info.Node.Type.Name;
            });
    }
}