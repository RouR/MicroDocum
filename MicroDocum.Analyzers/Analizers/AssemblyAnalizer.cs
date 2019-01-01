using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MicroDocum.Analyzers.Interfaces;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Analyzers.Analizers
{
    public class AssemblyAnalizer<T>
    {
        private readonly IAnalyzerTheme<T> _theme;

        public AssemblyAnalizer(IAnalyzerTheme<T> theme)
        {
            _theme = theme;
        }

        public static Type[] RemoveBaseTypes(Type[] types)
        {
            var ret = new List<Type>();
            ret.AddRange(types);

            foreach (var type in types)
            {
                foreach (var parent in types.Where(x=> x != type))
                {
                    if (type.CheckIsAssignableFrom(parent))
                        ret.Remove(parent);
                }
            }

            return ret.ToArray();
        }


        public IChain<MessageInfo, LinkInfo<T>> Analize(Assembly[] assemblies, Type[] attributes,
            Func<Type, bool> messageTypeFilter = null)
        {
            var result = new Chain<MessageInfo, LinkInfo<T>>();

            if (attributes == null)
                throw new ArgumentNullException(nameof(attributes)); //attributes = Core.Defaults.GetAvailableAttributes();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                if (messageTypeFilter != null)
                    types = types.Where(messageTypeFilter).ToArray();

                var messages = types.Where(t => attributes.Any(a => CompilerUtils.MessageTypeHasAttribute(t, a)))
                    .ToArray();
                if (messages.Any())
                {
                    foreach (var message in messages)
                    {
                        result.AddNode(new ChainNode<MessageInfo>(message.FullName, new MessageInfo()
                        {
                            Type = message,
                            Attributes = CompilerUtils.GetAttributes(message),
                            Interfaces = RemoveBaseTypes(CompilerUtils.GetLinks(message))
                        }));
                    }
                }
            }
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                if (messageTypeFilter != null)
                    types = types.Where(messageTypeFilter).ToArray();

                var messages = types.Where(t => attributes.Any(a => CompilerUtils.MessageTypeHasAttribute(t, a)))
                    .ToArray();
                if (messages.Any())
                {
                    foreach (var message in messages)
                    {
                        var interfaces = CompilerUtils.GetLinks(message);
                        interfaces = RemoveBaseTypes(interfaces);

                        var msgAttributes = CompilerUtils.GetAttributes(message);
                        
                        var links = _theme.GetThemedLinks(message, msgAttributes, interfaces, result);

                        if(links == null || links.Length == 0)
                            continue;

                        foreach (var linkMetadata in links)
                        {
                            result.AddDirectedEdge(result[message.FullName], result[linkMetadata.ToMessage.FullName], linkMetadata.Link);
                        }
                        
                       
                    }
                }
            }
            return result;
        }
    }
}