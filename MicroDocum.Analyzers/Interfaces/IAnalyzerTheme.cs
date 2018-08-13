using System;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Analyzers.Interfaces
{
    public interface IAnalyzerTheme<TLinkStyle>
    {
        Type[] GetAvailableThemeAttributes();

        LinkMetadata<TLinkStyle>[] GetThemedLinks(Type iface, Type fromMessage);
       
    }
}
