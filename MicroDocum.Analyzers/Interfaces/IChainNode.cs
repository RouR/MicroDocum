namespace MicroDocum.Analyzers.Interfaces
{
    public interface IChainNode<out TK>
    {
        string Id { get; }
        TK Node { get; }

        bool IsLoop { get; set; }
    }
}