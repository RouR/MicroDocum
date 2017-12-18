using MicroDocum.Analyzers;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz;
using MicroDocum.Graphviz.Enums;
using MicroDocum.Graphviz.Models;

namespace MicroDocum.Themes.DefaultTheme
{
    public static class ThemeLinkRules
    {
        public static readonly LinkRule<LinkInfo<DefaultLinkStyle>> ErrorIsRed = new LinkRule<LinkInfo<DefaultLinkStyle>>((edge, attributes, map) =>
        {
            if (map[edge.FromId].IsLoop)
                attributes.SetValue(GraphvizAttribute.Color.ToString(), "red");
        });

        public static readonly LinkRule<LinkInfo<DefaultLinkStyle>> DefaultColors = new LinkRule<LinkInfo<DefaultLinkStyle>>((edge, attributes, map) =>
        {
            if (map[edge.FromId].IsLoop)
                attributes.SetValue(GraphvizAttribute.Color.ToString(), "red");
            else
            {
                if (edge.Info.Style.CheckHasFlag(DefaultLinkStyle.Obsolete))
                    attributes.SetValue(GraphvizAttribute.Color.ToString(), "gray");
                else
                if (edge.Info.Style.CheckHasFlag(DefaultLinkStyle.Highlight))
                    attributes.SetValue(GraphvizAttribute.Color.ToString(), "darkorange");
                else
                    attributes.SetValue(GraphvizAttribute.Color.ToString(), edge.Info.Style.CheckHasFlag(DefaultLinkStyle.AlternativeColor) ? "darkorchid4" : "black");
            }
        });

        public static readonly LinkRule<LinkInfo<DefaultLinkStyle>> DefaultStyles = new LinkRule<LinkInfo<DefaultLinkStyle>>((edge, attributes, map) =>
        {
            if (edge.Info.Style.CheckHasFlag(DefaultLinkStyle.Thin))
                attributes.SetValue(GraphvizAttribute.Style.ToString(), GraphvizStyle.Dashed.ToString().ToLower());

            if (edge.Info.Style.CheckHasFlag(DefaultLinkStyle.Bold))
            {
                attributes.SetValue(GraphvizAttribute.Style.ToString(), GraphvizStyle.Bold.ToString());
                attributes.SetValue(GraphvizAttribute.Arrowhead.ToString(), GraphvizArrowType.Diamond.ToString().ToLower());
            }

            if (edge.Info.Style.CheckHasFlag(DefaultLinkStyle.Dot))
                attributes.SetValue(GraphvizAttribute.Arrowhead.ToString(), GraphvizArrowType.Empty.ToString().ToLower());

        });
    }
}