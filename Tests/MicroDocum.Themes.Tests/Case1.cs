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
    public class Case1 
    {
        //must be used at least 2 times
        private const string Endpoint1Testname = "ServiceNameTestName";

        [LabelAlt("altLabel1")]
        [TTL(1.625)]
        [ServiceName(Endpoint1Testname)]
        public class Class1 : IProduce<Struct1>, IProduceOnce<IInterface1>
        {
        }

        [LabelAlt("<<table border='1' cellborder='0'><tr><td align='center'><font color='black' point-size='14'>altLabel2</font></td></tr><tr><td align='right'><font color='red' point-size='6'>some notice</font></td></tr></table>>")]
        [TagsAlt("altTag1", "altTag2")]
        public struct Struct1 : IProduceStrong<IInterface1>
        {
        }
        
        [Tags("tag1", "tag22")]
        [TagsAlt("altTag0")]
        [TTL(33)]
        [ServiceName(Endpoint1Testname)]
        public interface IInterface1 : IProduceWeak<Class1>
        {
        }

        private readonly IGraphvizGenerator<DefaultLinkStyle> _gen = new GraphvizDotGenerator<DefaultLinkStyle>(new DefaultTheme.DefaultTheme());
        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";

        [Test]
        public void Case1_Should_GenerateFile()
        {
            //Given
            var theme = new DefaultTheme.DefaultTheme();
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
            var theme = new DefaultTheme.DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            StringAssert.Contains("33", graphwizFileData);
        }

        [Test]
        public void Case1_Should_HaveEndpointAttribute()
        {
            //Given
            var theme = new DefaultTheme.DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            StringAssert.Contains("rank=same", graphwizFileData);
            StringAssert.Contains(Endpoint1Testname, graphwizFileData);
        }

        [Test]
        public void Case1_Should_HaveTagAttribute()
        {
            //Given
            var theme = new DefaultTheme.DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            StringAssert.Contains("tag1", graphwizFileData);
            StringAssert.Contains("tag22", graphwizFileData);
        }

        [Test]
        public void Case1_Should_HaveAltTagAttribute()
        {
            //Given
            var theme = new DefaultTheme.DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            StringAssert.Contains("altTag0", graphwizFileData);
            StringAssert.Contains("altTag1", graphwizFileData);
            StringAssert.Contains("altTag2", graphwizFileData);
        }

        [Test]
        public void Case1_Should_HaveAltLabelAttribute()
        {
            //Given
            var theme = new DefaultTheme.DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            StringAssert.Contains("altLabel1", graphwizFileData);
            StringAssert.Contains("altLabel2", graphwizFileData);
        }

    }
}
