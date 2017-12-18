using System;
using System.Collections.Generic;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz.Interfaces;
using MicroDocum.Graphviz.Models;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace MicroDocum.Themes.DefaultTheme
{
    public class DefaultTheme: IGraphvizTheme<DefaultLinkStyle>
    {
        public Type[] GetAvailableThemeAttributes()
        {
            return new[]
            {
                typeof(ServiceNameAttribute),
                typeof(LabelAltAttribute),
                typeof(TagsAltAttribute),
                typeof(TagsAttribute),
                typeof(TTLAttribute)
            };
        }

        public LinkInfo<DefaultLinkStyle> DefaultRules(LinkMetadata meta)
        {
            var dict = new Dictionary<Type, LinkInfo<DefaultLinkStyle>>
            {
                {
                    typeof(IProduce<>), new LinkInfo<DefaultLinkStyle>
                    {
                        Style = DefaultLinkStyle.Default
                    }
                },
                {
                    typeof(IProduceOnce<>), new LinkInfo<DefaultLinkStyle>
                    {
                        Style = DefaultLinkStyle.AlternativeColor,
                        Label = "Once"
                    }
                },
                {
                    typeof(IProduceSometimes<>), new LinkInfo<DefaultLinkStyle>
                    {
                        Style = DefaultLinkStyle.AlternativeColor | DefaultLinkStyle.Dot,
                        Label = "Sometimes"
                    }
                },
                {
                    typeof(IProduceStrong<>), new LinkInfo<DefaultLinkStyle>
                    {
                        Style = DefaultLinkStyle.Default | DefaultLinkStyle.Bold
                    }
                },
                {
                    typeof(IProduceWeak<>), new LinkInfo<DefaultLinkStyle>
                    {
                        Style = DefaultLinkStyle.Default | DefaultLinkStyle.Thin
                    }
                }
            };

            var result = new LinkInfo<DefaultLinkStyle>()
            {
                Style = DefaultLinkStyle.Default
            };

            if (dict.ContainsKey(meta.Link))
                result = dict[meta.Link];

            return result;
        }

        public IList<LinkRule<LinkInfo<DefaultLinkStyle>>> DefaultLinkRules()
        {
            return new List<LinkRule<LinkInfo<DefaultLinkStyle>>>()
            {
                ThemeLinkRules.DefaultColors,
                ThemeLinkRules.DefaultStyles
            };
        }


        IList<NodeRule<MessageInfo, DefaultLinkStyle>> IGraphvizTheme<DefaultLinkStyle>.DefaultNodeRules()
        {
            return new List<NodeRule<MessageInfo, DefaultLinkStyle>>()
            {
                ThemeNodeRules.SetComments,
                ThemeNodeRules.SetLabel,
                TTLAttribute.GetRule(),
                ServiceNameAttribute.GetRule(),
                TagsAttribute.GetRule(),
                TagsAltAttribute.GetRule(),
                LabelAltAttribute.GetRule(),
            };
        }

        public IList<SpecialRule<MessageInfo, DefaultLinkStyle>> SpecialRulesPreProcess()
        {
            return new List<SpecialRule<MessageInfo, DefaultLinkStyle>>()
            {
                new SpecialRule<MessageInfo, DefaultLinkStyle>(map => "rankdir=LR; \n"),
                ServiceNameAttribute.GetPreProcessRule(),
                new SpecialRule<MessageInfo, DefaultLinkStyle>(map => "forcelabels=true;\n"),
            };
        }

        public IList<SpecialRule<MessageInfo, DefaultLinkStyle>> SpecialRulesPostProcess()
        {
            return new List<SpecialRule<MessageInfo, DefaultLinkStyle>>()
            {
                ServiceNameAttribute.GetPostProcessRule()
            };
        }

        public bool BorderedChains()
        {
            //It can work wrong with rule of ServiceNameAttribute due to additional subgraphs;
            return false;
        }

        /// <summary>
        /// HTML-Like Labels
        /// https://graphviz.gitlab.io/_pages/doc/info/shapes.html#html
        /// </summary>
        /// <param name="labelText"></param>
        /// <returns></returns>
        public static string CheckAndWrapToHtmlTable(string labelText)
        {
            if (!labelText.StartsWith("<<table",StringComparison.CurrentCultureIgnoreCase))
                return WrapToHtmlTable(labelText);
            else
                return labelText;
        }

        private static string WrapToHtmlTable(string labelText)
        {
            return $"<<table border='0' cellborder='0'><tr><td align='center'><font color='black' point-size='14'>{labelText.Replace("\n","<BR/>")}</font></td></tr></table>>";
        }

        public static string AddNewRow(string labelText, string rowText)
        {
            labelText = CheckAndWrapToHtmlTable(labelText);
            var position = labelText.LastIndexOf("</table>", StringComparison.CurrentCultureIgnoreCase);
            labelText = labelText.Insert(position, $"<tr><td align='right'>{rowText}</td></tr>");
            return labelText;
        }

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static Dictionary<string,List<string>> ServiceNames = new Dictionary<string, List<string>>();
    }
}
