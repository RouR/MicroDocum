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

namespace MicroDocum.Net35.Tests
{
    [TestFixture]
    public class Case1 
    {
        [Tags("tag1", "tag2")]
        [TagsAlt("alttag")]
        [TTL(1.625)]
        [ServiceName("EndpointName")]
        public class Class1 : IProduce<Struct1>, IProduceOnce<IInterface1>
        {
        }

        [LabelAlt("altLabel")]
        [Tags("tag1", "tag22")]
        [TagsAlt]
        [TTL(0)]
        [ServiceName("EndpointName")]
        public struct Struct1 : IProduceStrong<IInterface1>
        {
        }

        [Tags("tag33", "tag2")]
        [TagsAlt]
        [TTL(33)]
        [ServiceName("EndpointName")]
        public interface IInterface1 : IProduceWeak<Class1>
        {
        }

        private readonly IGraphvizGenerator<DefaultLinkStyle> _gen = new GraphvizDotGenerator<DefaultLinkStyle>(new DefaultTheme());
        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";

        [Test]
        public void Case1_Should_GenerateFile()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            Assume.That(c.Nodes.Count == 3);
            Assume.That(c.Edges, Has.Count.AtLeast(4));
            Assume.That(c.GetSingles(), Has.Count.EqualTo(0), "GetSingles");
            Assume.That(c.GetHeads(), Has.Count.EqualTo(1), "GetHeads");
            Assume.That(c.GetLeafs(), Has.Count.EqualTo(0), "GetLeafs");
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            Assert.NotNull(graphwizFileData);
            Console.WriteLine(graphwizFileData);
        }

        [Test]
        public void Case1_Should_HaveTTLAttribute()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            StringAssert.Contains("33", graphwizFileData);
        }



    }
}
