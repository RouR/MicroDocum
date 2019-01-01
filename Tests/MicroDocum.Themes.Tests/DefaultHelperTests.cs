using System;
using System.Linq;
using MicroDocum.Graphviz.Interfaces;
using MicroDocum.Themes.DefaultTheme;
using NUnit.Framework;

namespace MicroDocum.Themes.Tests
{
    [TestFixture]
    public class DefaultHelperTests
    {
        private readonly IGraphvizTheme<DefaultLinkStyle> _theme = new DefaultTheme.DefaultTheme();
        [Test]
        public void GetAvailableAttributes_Should_EnumerateAllAvailableAttributes()
        {
            //Given
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.FullName.StartsWith("MicroDocum.Themes,"));
            var defined = assembly.DefinedTypes.Where(x => x.Name.EndsWith("Attribute") && !x.Name.StartsWith("Produce")).ToArray();
            Assume.That(defined.Length >= 5, defined.Length.ToString());
            //When
            var actual = _theme.GetAvailableThemeAttributes();
            //Then
            Assert.IsNotNull(actual);
            Assert.AreEqual(defined.Length, actual.Length);
            CollectionAssert.AreEquivalent(defined, actual);
        }
    }
}