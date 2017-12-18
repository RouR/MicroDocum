using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MicroDocum.Analyzers.Models
{
    public struct LinkMetadata
    {
        public Type Link { get; set; }
        public Type FromMessage { get; set; }
        public Type ToMessage { get; set; }
    }
}