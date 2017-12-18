using MicroDocum.Analyzers.Interfaces;

namespace MicroDocum.Analyzers.Models
{
    public struct EdgeType<T> : IEdgeInfo<T>
    {
        public string FromId { get; set; }
        public string ToId { get; set; }
        public T Info { get; set; }

        public EdgeType(string from, string to, T data)
        {
            FromId = from;
            ToId = to;
            Info = data;
        }
    }
}