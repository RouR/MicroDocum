using System;
using System.Collections;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Analyzers.Interfaces
{
    public interface IAnalyzerTheme<TLinkStyle>
    {
        Type[] GetAvailableThemeAttributes();

        LinkMetadata<TLinkStyle>[] GetThemedLinks(Type dto, Attribute[] attributes, Type[] interfaces, Chain<MessageInfo, LinkInfo<TLinkStyle>> map);
    }
}
