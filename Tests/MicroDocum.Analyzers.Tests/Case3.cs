using System;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Themes.DefaultTheme;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
using NUnit.Framework;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedTypeParameter

namespace MicroDocum.Analyzers.Tests
{
    [TestFixture]
    public class Case3
    {
        public interface IWrongInterface1
        {
        }
        public interface IWrongInterface2<K,V>
        {
        }

        public class RootClass: IWrongInterface1
        {
            
        }

        public class ParetnClass : RootClass, IWrongInterface2<Class1,string>
        {

        }

        [ServiceName("Case3")]
        public class Class1 : IWrongInterface1, IProduce<Class2>
        {
        }

        [ServiceName("Case3")]
        // ReSharper disable once RedundantExtendsListEntry
        public class Class2 : IWrongInterface2<Class1,string>, IProduceSometimes<ClassEnd>
        {
        }

        [ServiceName("Case3")]
        // ReSharper disable once ClassNeverInstantiated.Global
        public class ClassEnd
        {
        }

        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";
        [Test]
        public void Case3_Should_SkipWrongInterfaces()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            //When
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(),  t => t.FullName?.StartsWith(_classname) ?? false);
            //Then
            Assert.That(c.Nodes, Has.Exactly(3).Items);
            Assert.That(c.Edges, Has.Count.EqualTo(2));
            Assert.That(c.GetSingles(), Has.Count.EqualTo(0), "GetSingles");
            Assert.That(c.GetHeads(), Has.Count.EqualTo(1), "GetHeads");
            Assert.That(c.GetLeafs(), Has.Count.EqualTo(1), "GetLeafs");
        }
    }
}
