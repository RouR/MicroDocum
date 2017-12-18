using System;

namespace MicroDocum.Analyzers
{
    public sealed class UnknownNodeException : Exception
    {
        public UnknownNodeException(string key) : base($"The node is not exist. (key '{key}'). Can`t create edge to that node.")
        {
        }
    }
}