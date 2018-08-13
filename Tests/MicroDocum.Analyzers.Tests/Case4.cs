using System;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Themes.DefaultTheme;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
using NUnit.Framework;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace MicroDocum.Analyzers.Tests
{
    [TestFixture]
    public class Case4
    {
        [TTL(1.625)]
        [ServiceName("EndpointName")]
        public class Class1 : IProduce<Struct1, IInterface1>
        {
        }

        [Tags("tag1", "tag22")]
        [TagsAlt]
        [TTL(1)]
        [ServiceName("EndpointName2")]
        public struct Struct1 : IProduceStrong<IInterface1>
        {
        }

        [LabelAlt("altLabel")]
        [TTL(1)]
        [ServiceName("EndpointName2")]
        public interface IInterface1 : IProduceWeak<Class1>
        {
        }

        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";
        [Test]
        public void Case1_Should_FindMessageTypes()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            //When
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(/*"MicroDocum.Analyzers.Tests.Case1+"*/_classname) ?? false);
            //Then
            Assert.That(c.Nodes, Has.Exactly(3).Items);
            Assert.That(c.Edges, Has.Count.AtLeast(4));
            Assert.That(c.GetSingles(), Has.Count.EqualTo(0), "GetSingles");
            Assert.That(c.GetHeads(), Has.Count.EqualTo(1), "GetHeads");
            Assert.That(c.GetLeafs(), Has.Count.EqualTo(0), "GetLeafs");
        }
    }
}
