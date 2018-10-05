using System;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Graphviz;
using MicroDocum.Graphviz.Interfaces;
using MicroDocum.Themes.DefaultTheme;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
using NUnit.Framework;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace MicroDocum.Themes.Tests
{
    [TestFixture]
    public class Case2
    {
        [ServiceName("EndpointName")]
        public class Class1 : IProduce<Class21,Class22>
        {

        }
        [ServiceName("EndpointName")]
        public class Class21
        {

        }
        [ServiceName("EndpointName")]
        public class Class22
        {

        }

        private readonly IGraphvizGenerator<DefaultLinkStyle> _gen = new GraphvizDotGenerator<DefaultLinkStyle>(new DefaultTheme.DefaultTheme());
        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";

        [Test]
        public void Case2_Should_detectMultiGeneric()
        {
            //Given
            var theme = new DefaultTheme.DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            //When
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //Then
            Assert.AreEqual(3, c.Nodes.Count);
            Assert.AreEqual(2, c.Edges.Count);
        }

       
    }
}
