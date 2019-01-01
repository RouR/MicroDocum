using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    public class Case7
    {
#region DTO
        [ServiceName("Account")]
        public class RegisterRequest : IProduce<CreateUserRequest>
        {
        
        }
        [ServiceName("Account")]
        public class RegisterResponse: IErrorable<Guid>
        {
            public bool HasError { get; set; }
            public string Message { get; set; }
            /// <summary>
            /// UserId
            /// </summary>
            public Guid Data { get; set; }
        }
        [ServiceName("Account")]
        [ProduceDTO(typeof(CreateUserResponse))]
        public class CreateUserRequest
        {
        
        }
        [ServiceName("Account")]
        [ProduceDTO(typeof(RegisterResponse))]
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
        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";
        [Test]
        public void Case7_Should_FindMessageTypes()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            //When
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(/*"MicroDocum.Analyzers.Tests.Case1+"*/_classname) ?? false);
            //Then
            Assert.That(c.Nodes.Count, Is.EqualTo(4));
            Assert.That(c.Edges.Count, Is.EqualTo(3));
            Assert.That(c.GetSingles(), Has.Count.EqualTo(0), "GetSingles");
            Assert.That(c.GetHeads(), Has.Count.EqualTo(1), "GetHeads");
            Assert.That(c.GetLeafs(), Has.Count.EqualTo(1), "GetLeafs");
        }

        [Test]
        public void Case7_Should_SplitChains()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(/*"MicroDocum.Analyzers.Tests.Case1+"*/_classname) ?? false);
            //When
            var chains = c.SplitChains();
            //Then
            Assert.That(chains, Has.Count.EqualTo(1));
        }
    }
}
