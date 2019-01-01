using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using MicroDocum.Analyzers.Analizers;
using MicroDocum.Graphviz.Interfaces;
using MicroDocum.Themes.DefaultTheme;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
using NUnit.Framework;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace MicroDocum.Graphviz.Tests
{
    [TestFixture]
    public class Case2 : GraphVizWrapperTests
    {
        #region DTO
        [ServiceName("Register")]
        [ProduceDTO(typeof(CreateUserRequest))]
        public class RegisterRequest : IProduce<RegisterResponse>
        {
        
        }
        [ServiceName("Register")]
        public class RegisterResponse: IErrorable<Guid>
        {
            public bool HasError { get; set; }
            public string Message { get; set; }
            /// <summary>
            /// UserId
            /// </summary>
            public Guid Data { get; set; }
        }
        [ServiceName("User")]
        [ProduceDTO(typeof(CreateUserResponse))]
        public class CreateUserRequest
        {
        
        }
        [ServiceName("User")]
        public class CreateUserResponse: IErrorable<UserEntity>
        {
            public bool HasError { get; set; }
            public string Message { get; set; }
            public UserEntity Data { get; set; }
        }

        interface IErrorable<T>
        {
            bool HasError { get; set; }
            string Message { get; set; }
            T Data { get; set; }
        }

        public class UserEntity: DBEntity, IUser
        {
            public Guid UserId { get; set; }
        }

        public abstract class DBEntity : ITimestampedEntity, IPublicIdEntity
        {
            [IgnoreDataMember]
            long Id { get; set; }

            public DateTime Created { get; set; }
            public DateTime Updated { get; set; }
            public Guid PublicId { get; set; }
        }
        public interface IPublicIdEntity
        {
            Guid PublicId { get; set; }
        }
        public interface ITimestampedEntity
        {
            DateTime Created { get; set; }
            DateTime Updated { get; set; }
        }
        public interface IUser
        {
            Guid UserId { get; set; }
        }
     

    
        #endregion

        private readonly IGraphvizGenerator<DefaultLinkStyle> _gen = new GraphvizDotGenerator<DefaultLinkStyle>(new DefaultTheme());
        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";

        [Test]
        public void Case2_Should_GenerateFile()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            Assume.That(c.Nodes.Count, Is.EqualTo(4), "Nodes");
            Assume.That(c.Edges.Count, Is.EqualTo(3), "Edges");
            Assume.That(c.GetSingles().Count, Is.EqualTo(0), "GetSingles");
            Assume.That(c.GetHeads().Count, Is.EqualTo(1), "GetHeads");
            Assume.That(c.GetLeafs().Count, Is.EqualTo(2), "GetLeafs");
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            DrawAnSave(graphwizFileData);
            //Console.WriteLine(graphwizFileData);
        }


        [Test]
        public void Case2_Should_Have2Cluster()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
            //When
            var graphwizFileData = _gen.Generate(c);
            //Then
            Assert.AreEqual(2, Regex.Matches(Regex.Escape(graphwizFileData), "cluster_").Count);
        }

    }
}
