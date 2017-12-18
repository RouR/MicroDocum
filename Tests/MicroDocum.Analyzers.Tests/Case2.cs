using System;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Themes.DefaultTheme;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
using NUnit.Framework;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace MicroDocum.Analyzers.Tests
{
    [TestFixture]
    public class Case2
    {
        [TTL(1)]
        [ServiceName("EndpointName")]
        public class Class1 : IProduce<Class2>
        {
        }

        [LabelAlt("altLabel")]
        [Tags("tag1", "tag22")]
        [TagsAlt]
        [TTL(1)]
        public class Class2
        {
        }

        [TagsAlt]
        [TTL(1)]
        public class Class3
        {
        }


        [LabelAlt("altLabel2")]
        [Tags("tag1", "tag22")]
        [TagsAlt("alt")]
        [TTL(1)]
        [ServiceName("EndpointName")]
        public class Class4
        {
        }
        /// <summary>
        /// "MicroDocum.Analyzers.Tests.Case2+"
        /// </summary>
        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";
        [Test]
        public void Case2_Should_FindMessageTypes()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            //When
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //Then
            Assert.That(c.Nodes, Has.Exactly(4).Items);
            Assert.That(c.Edges, Has.Count.EqualTo(1));
            Assert.That(c.GetSingles(), Has.Count.EqualTo(2), "GetSingles");
            Assert.That(c.GetHeads(), Has.Count.EqualTo(1), "GetHeads");
            Assert.That(c.GetLeafs(), Has.Count.EqualTo(1), "GetLeafs");
        }
    }
}
