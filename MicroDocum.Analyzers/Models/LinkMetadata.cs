using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MicroDocum.Analyzers.Models
{
    public struct LinkMetadata<T>
    {
        public LinkInfo<T> Link { get; set; }
        public Type FromMessage { get; set; }
        public Type ToMessage { get; set; }
    }
}