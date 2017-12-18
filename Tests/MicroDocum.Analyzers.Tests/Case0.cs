using MicroDocum.Analyzers.Analizers;
using NUnit.Framework;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedTypeParameter

namespace MicroDocum.Analyzers.Tests
{
    [TestFixture]
    public class Case0
    {
        public interface IBase<T>
        {

        }

        public interface IChild<T> : IBase<T>
        {

        }

        public class Class1 : IChild<ClassEnd>
        {
        }

        // ReSharper disable once RedundantExtendsListEntry
        public class Class2 : IBase<ClassEnd>, IChild<ClassEnd>
        {
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        public class ClassEnd
        {
        }


        [Test]
        public void Case0_Should_RemoveBaseTypes1()
        {
            //Given
            var types = CompilerUtils.GetLinks(typeof(Class1));
            Assume.That(types, Has.Exactly(2).Items);
            //When
            var interfaces = AssemblyAnalizer<object>.RemoveBaseTypes(types);
            //Then
            Assert.That(interfaces, Has.Exactly(1).Items);
        }

        [Test]
        public void Case0_Should_RemoveBaseTypes2()
        {
            //Given
            var types = CompilerUtils.GetLinks(typeof(Class2));
            Assume.That(types, Has.Exactly(2).Items);
            //When
            var interfaces = AssemblyAnalizer<object>.RemoveBaseTypes(types);
            //Then
            Assert.That(interfaces, Has.Exactly(1).Items);
        }
    }
}
