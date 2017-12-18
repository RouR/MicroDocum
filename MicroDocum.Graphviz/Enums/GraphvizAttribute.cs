// ReSharper disable UnusedMember.Global
namespace MicroDocum.Graphviz.Enums
{
    /// <summary>
    /// http://www.graphviz.org/doc/info/attrs.html
    /// </summary>
    public enum GraphvizAttribute
    {
        Label,
        /// <summary>
        /// http://www.graphviz.org/doc/info/colors.html
        /// </summary>
        Color,
        Fillcolor,
        Style,
        Arrowhead,
        Arrowtail,
        /// <summary>
        /// External label for a node or edge. For nodes, the label will be placed outside of the node but near it. For edges, the label will be placed near the center of the edge. 
        /// These labels are added after all nodes and edges have been placed. The labels will be placed so that they do not overlap any node or label. 
        /// This means it may not be possible to place all of them. To force placing all of them, use the forcelabels attribute.
        /// https://graphviz.gitlab.io/_pages/doc/info/attrs.html#d:xlabel
        /// </summary>
        Xlabel,
        /// <summary>
        /// A string specifying the shape of a node. There are three main types of shapes : polygon-based, record-based and user-defined. 
        /// https://graphviz.gitlab.io/_pages/doc/info/shapes.html#record
        /// </summary>
        Shape,
    }
}