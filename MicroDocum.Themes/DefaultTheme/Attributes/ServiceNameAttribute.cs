using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroDocum.Analyzers;
using MicroDocum.Analyzers.Models;
using MicroDocum.Graphviz;
using MicroDocum.Graphviz.Models;

namespace MicroDocum.Themes.DefaultTheme.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class ServiceNameAttribute : Attribute
    {
        private readonly string _groupName;

        public ServiceNameAttribute(string groupName)
        {
            if(string.IsNullOrEmpty(groupName))
                throw new ArgumentNullException(groupName);

            _groupName = groupName;
        }

        public static SpecialRule<MessageInfo, DefaultLinkStyle> GetPreProcessRule()
        {
            return new SpecialRule<MessageInfo, DefaultLinkStyle>((map) =>
            {
                DefaultTheme.ServiceNames.Clear();
                return null;
            });
        }

        public static NodeRule<MessageInfo, DefaultLinkStyle> GetRule()
        {
            return new NodeRule<MessageInfo, DefaultLinkStyle>((node, nodeRules, map) =>
            {
                var attr = node.Info.Node.Attributes.FirstOrDefault(x => CompilerUtils.AttributeIs(x, typeof(ServiceNameAttribute)));
                if (attr != null)
                {
                    var a = (ServiceNameAttribute)attr;
                    var rank = (string)a._groupName;

                    DefaultTheme.ServiceNames.SetValueIfNotExists(rank, new List<string>());
                    DefaultTheme.ServiceNames[rank].Add(node.Name);

                }
            });
        }

        public static SpecialRule<MessageInfo, DefaultLinkStyle> GetPostProcessRule()
        {
            return new SpecialRule<MessageInfo, DefaultLinkStyle>((map) =>
            {
                var sb = new StringBuilder();

                foreach (var sname in DefaultTheme.ServiceNames)
                {
                    if (sname.Value.Count >= 2)
                    {
                        sb.Append("\n{");
                        sb.Append($"/* ServiceName {sname.Key}*/ rank=same;");
                        sb.Append(string.Join(", ", sname.Value.Distinct().ToArray()));
                        sb.Append("}\n");
                    }
                }

                return sb.ToString();
            });
        }
    }
}