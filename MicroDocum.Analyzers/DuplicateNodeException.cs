using System;

namespace MicroDocum.Analyzers
{
    public sealed class DuplicateNodeException : Exception
    {
        public DuplicateNodeException(string key) : base($"The node is already exist. (key '{key}')")
        {
        }
    }
}
