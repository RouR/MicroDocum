using System;
using System.Linq;
using MicroDocum.Analyzers;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz.Enums;
using MicroDocum.Graphviz.Models;

namespace MicroDocum.Themes.DefaultTheme.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class TagsAttribute : Attribute
    {
        private readonly string[] _tags;

        //At leat 1 tag required
        public TagsAttribute(string tag, params string[] tags)
        {
            _tags = (tags?.Where(x => !string.IsNullOrEmpty(x)) ?? new string[0]).Union(new []{tag}).Distinct().OrderBy(x=>x).ToArray();
        }

        public static NodeRule<MessageInfo, DefaultLinkStyle> GetRule()
        {
            return new NodeRule<MessageInfo, DefaultLinkStyle>((node, nodeRules, map) =>
            {
                var attr = node.Info.Node.Attributes.FirstOrDefault(x => CompilerUtils.AttributeIs(x, typeof(TagsAttribute)));
                if (attr != null)
                {
                    var a = (TagsAttribute)attr;
                    var tags = a._tags;
                    
                    if (tags.Any())
                    {
                        var labelText = node.Attributes[GraphvizAttribute.Label.ToString()];
                        labelText = DefaultTheme.CheckAndWrapToHtmlTable(labelText);
                        var newRowText = $"<font color='gray' point-size='6'>{string.Join(", ", tags.Select(x=>"#"+x).ToArray())}</font>";
                        labelText = DefaultTheme.AddNewRow(labelText, newRowText);
                        node.Attributes[GraphvizAttribute.Label.ToString()] = labelText;
                    }
                }
            });
        }
    }
}