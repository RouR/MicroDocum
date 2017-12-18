namespace MicroDocum.Analyzers.Interfaces
{
    public interface IEdgeInfo<T>
    {
        string FromId { get; set; }
        T Info { get; set; }
        string ToId { get; set; }
    }
}