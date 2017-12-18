using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MicroDocum.Analyzers.Models
{
    public struct MessageInfo
    {
        public Type Type { get; set; }
        public Attribute[] Attributes { get; set; }
        public Type[] Interfaces { get; set; }
    }
}
