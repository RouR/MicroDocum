using System;
using System.Linq;
using MicroDocum.Analyzers;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz.Enums;
using MicroDocum.Graphviz.Models;

// ReSharper disable InconsistentNaming
namespace MicroDocum.Themes.DefaultTheme.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class TTLAttribute : Attribute
    {
        private readonly double _seconds;

        public TTLAttribute(double seconds)
        {
            _seconds = seconds;
        }

        public static NodeRule<MessageInfo, DefaultLinkStyle> GetRule()
        {
            return new NodeRule<MessageInfo, DefaultLinkStyle>((node, nodeRules, map) =>
            {
                var attr = node.Info.Node.Attributes.FirstOrDefault(x => CompilerUtils.AttributeIs(x, typeof(TTLAttribute)));
                if (attr != null)
                {
                    var a = (TTLAttribute) attr;
                    var seconds = (double)a._seconds;
                    var val = TimeSpan.FromSeconds(seconds);
                    if (val.TotalMilliseconds >= 1)
                    {
                        var labelText = node.Attributes[GraphvizAttribute.Label.ToString()];
                        labelText = DefaultTheme.CheckAndWrapToHtmlTable(labelText);
                        var newRowText = $"<font color='red' point-size='6'>TTL {val.TotalSeconds:F2} sec</font>";
                        labelText = DefaultTheme.AddNewRow(labelText, newRowText);
                        node.Attributes[GraphvizAttribute.Label.ToString()] = labelText;
                    }
                }
            });
        }
    }
}