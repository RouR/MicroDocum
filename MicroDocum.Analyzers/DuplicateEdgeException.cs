using System;

namespace MicroDocum.Analyzers
{
    public sealed class DuplicateEdgeException : Exception
    {
        public DuplicateEdgeException(string from, string to) : base($"The edge is already exist. ({from} -> {to})")
        {
        }
    }
}