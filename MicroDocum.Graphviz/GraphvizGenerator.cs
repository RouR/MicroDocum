using MicroDocum.Graphviz.Enums;

namespace MicroDocum.Graphviz
{
    public abstract class GraphvizGenerator 
    {
        private readonly GraphRenderingEngine _engineType;

        protected GraphvizGenerator(GraphRenderingEngine engineType)
        {
            _engineType = engineType;
        }

        protected string GetAbout()
        {
            return $"\n//GraphViz Engine: {_engineType.ToString()}" +
                   "\n//You can generate image by online generators, for example - https://dreampuf.github.io/GraphvizOnline/";
        }
    }
}
