using System;
using System.Linq;
using MicroDocum.Analyzers;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz;
using MicroDocum.Graphviz.Enums;
using MicroDocum.Graphviz.Models;

namespace MicroDocum.Themes.DefaultTheme.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class LabelAltAttribute : Attribute
    {
        private readonly string _label;

        // [LabelAlt("altLabel1")]
        // [LabelAlt("<<table border='1' cellborder='0'><tr><td align='center'><font color='black' point-size='14'>altLabel2</font></td></tr><tr><td align='right'><font color='red' point-size='6'>some notice</font></td></tr></table>>")]
        public LabelAltAttribute(string label)
        {
            _label = label;
        }

        public static NodeRule<MessageInfo, DefaultLinkStyle> GetRule()
        {
            return new NodeRule<MessageInfo, DefaultLinkStyle>((node, nodeRules, map) =>
            {
                var attr = node.Info.Node.Attributes.FirstOrDefault(x => CompilerUtils.AttributeIs(x, typeof(LabelAltAttribute)));
                if (attr != null)
                {
                    var a = (LabelAltAttribute)attr;
                    var label = a._label;
                    
                    if (null != label)
                    {
                        node.Attributes.SetValue(GraphvizAttribute.Xlabel.ToString(), label);
                    }
                }
            });
        }
    }
}
