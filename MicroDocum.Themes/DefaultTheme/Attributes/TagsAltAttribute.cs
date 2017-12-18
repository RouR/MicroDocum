using System;
using System.Linq;
using MicroDocum.Analyzers;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz.Enums;
using MicroDocum.Graphviz.Models;

namespace MicroDocum.Themes.DefaultTheme.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class TagsAltAttribute : Attribute
    {
        private readonly string[] _tags;

        public TagsAltAttribute(params string[] tags)
        {
            _tags = tags?.Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x).ToArray() ?? new string[0];
        }

        public static NodeRule<MessageInfo, DefaultLinkStyle> GetRule()
        {
            return new NodeRule<MessageInfo, DefaultLinkStyle>((node, nodeRules, map) =>
            {
                var attr = node.Info.Node.Attributes.FirstOrDefault(x => CompilerUtils.AttributeIs(x, typeof(TagsAltAttribute)));
                if (attr != null)
                {
                    var a = (TagsAltAttribute)attr;
                    var tags = a._tags;

                    if (tags.Any())
                    {
                        var labelText = node.Attributes[GraphvizAttribute.Label.ToString()];
                        labelText = DefaultTheme.CheckAndWrapToHtmlTable(labelText);
                        var newRowText = $"<font color='gray' point-size='6'><i>{string.Join(", ", tags.Select(x => "#" + x).ToArray())}</i></font>";
                        labelText = DefaultTheme.AddNewRow(labelText, newRowText);
                        node.Attributes[GraphvizAttribute.Label.ToString()] = labelText;
                    }
                }
            });
        }
    }
}