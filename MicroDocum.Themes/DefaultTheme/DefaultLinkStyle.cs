using System;

namespace MicroDocum.Themes.DefaultTheme
{
    [Flags]
    public enum DefaultLinkStyle
    {
        Default = 0,
        Bold = 0x01,
        Thin = 0x02,
        Dot = 0x04,
        AlternativeColor = 0x08,
        Obsolete = 0x16,
        Highlight = 0x32,
        Error = 0x64
    }
}
