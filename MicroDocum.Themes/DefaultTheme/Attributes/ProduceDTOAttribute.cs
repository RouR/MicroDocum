using System;
using System.Linq;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Themes.DefaultTheme.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class ProduceDTOAttribute : Attribute
    {
        private readonly string _type;

        public ProduceDTOAttribute(Type type)
        {
            _type = type.FullName;
        }

        public static Func<Attribute, Chain<MessageInfo, LinkInfo<DefaultLinkStyle>>, Type> GetLink()
        {
            return (attr, map) =>
            {
                var a = (ProduceDTOAttribute) attr;
                var link = (string) a._type;
                return map.Nodes.Single(x => x.Node.Type.FullName == link).Node.Type;
            };
        }
    }
}
