using System.Text.RegularExpressions;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Graphviz;
using MicroDocum.Graphviz.Interfaces;
using MicroDocum.Themes.DefaultTheme;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
using NUnit.Framework;
using System;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace MicroDocum.Std.Tests
{
    [TestFixture]
    public class Case1 
    {
        [Label]
        [Tags]
        [TagsAlt]
        [TTL(1.625)]
        [Endpoint]
        public class Class1 : IProduce<Struct1>, IProduceOnce<IInterface1>
        {
        }

        [Label]
        [Tags]
        [TagsAlt]
        [TTL(0)]
        [Endpoint]
        public struct Struct1 : IProduceStrong<IInterface1>
        {
        }

        [Label]
        [Tags]
        [TagsAlt]
        [TTL(33)]
        [Endpoint]
        public interface IInterface1 : IProduceWeak<Class1>
        {
        }

        private readonly IGraphvizGenerator<DefaultLinkStyle> _gen = new GraphvizDotGenerator<DefaultLinkStyle>(new DefaultTheme());
        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";



        [Test]
        public void Case1_Should_Have4Links()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            Assert.AreEqual(4, Regex.Matches(Regex.Escape(graphwizFileData), "->").Count);
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
            Console.WriteLine(graphwizFileData);
        }

    }
}
