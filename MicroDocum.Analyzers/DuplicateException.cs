using System;

namespace MicroDocum.Analyzers
{
    public sealed class DuplicateException : Exception
    {
        public DuplicateException(string key) : base($"The node is already exist. (key '{key}')")
        {
        }
    }
}
