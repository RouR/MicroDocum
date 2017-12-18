using System;
using MicroDocum.Analyzers.Models;

namespace MicroDocum.Analyzers.Interfaces
{
    public interface IAnalyzerTheme<TLinkStyle>
    {
        Type[] GetAvailableThemeAttributes();

        LinkInfo<TLinkStyle> DefaultRules(LinkMetadata meta);
       
    }
}
