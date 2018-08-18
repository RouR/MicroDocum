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
    public class Case5
    {
#region DTO
        [ServiceName("ToDoSrv")]
        private class CreateTODO : TodoEntity, IProduce<SaveTODOResponse>
        {
        }
        [ServiceName("ToDoSrv")]
        private class DeleteTODO : IPublicIdEntity, IProduce<DeleteTODOResponse>
        {
            public Guid PublicId { get; set; }
        }
        [ServiceName("ToDoSrv")]
        private class ListTODO : ListTODORequest, IUser, IProduce<ListTODOResponse>
        {
            public Guid UserId { get; set; }
        }
        [ServiceName("ToDoSrv")]
        private class UpdateTODO: TodoEntity, IProduce<SaveTODOResponse>
        {
        }

        [ServiceName("Web")]
        private class DeleteTODORequest: IPublicIdEntity, IProduce<DeleteTODO>
        {
            public Guid PublicId { get; set; }
        }
        [ServiceName("Web")]
        private class DeleteTODOResponse : IErrorable<bool>
        {
            public bool HasError { get; set; }
            public string Message { get; set; }
            public bool Data { get; set; }
        }
        [ServiceName("Web")]
        private class ListTODORequest : IPaginationSetting
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
        }
        [ServiceName("Web")]
        private class ListTODOResponse : Pagination<TodoEntity>
        {
            public ListTODOResponse(IEnumerable<TodoEntity> items, int totalItems, IPaginationSetting settings) : base(items, totalItems, settings)
            {
            }
        }
        [ServiceName("Web")]
        private class SaveTODORequest : IProduceSometimes<CreateTODO>, IProduceSometimes<UpdateTODO>
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }
        [ServiceName("Web")]
        private class SaveTODOResponse : IErrorable<TodoEntity>
        {
            public bool HasError { get; set; }
            public string Message { get; set; }
            public TodoEntity Data { get; set; }
        }
        interface IErrorable<T>
        {
            bool HasError { get; set; }
            string Message { get; set; }
            T Data { get; set; }
        }
        private class TodoEntity : DBEntity, IUser
        {
            public string Title { get; set; }
            public string Description { get; set; }
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
        public interface IPaginationSetting
        {
            int Page { get; set; }
            int PageSize { get; set; }
        }

        private class Pagination<T>
        {
            readonly IList<T> items;

            public int Page { get; private set; }
            public int PageSize { get; private set; }
            public int TotalItems { get; private set; }

            public IEnumerable<T> Items => items;

            public int TotalPages => (int) Math.Ceiling(((double) TotalItems) / PageSize);

            public int FirstItem => ((Page - 1) * PageSize) + 1;

            public int LastItem => FirstItem + items.Count - 1;

            public bool HasPreviousPage => Page > 1;

            public bool HasNextPage => Page < TotalPages;

            public Pagination(IEnumerable<T> items, int totalItems, IPaginationSetting settings)
            {
                if (items == null)
                    throw new ArgumentNullException(nameof(items));

                if (settings == null)
                    throw new ArgumentNullException(nameof(settings));

                var page = settings.Page;
                var pageSize = settings.PageSize;

                if (page < 1)
                    throw new ArgumentOutOfRangeException("page", page, "Value must be greater than zero.");

                if (pageSize < 1)
                    throw new ArgumentOutOfRangeException("page", page, "Value must be greater than zero.");

                if (totalItems < 0)
                    throw new ArgumentOutOfRangeException("page", page, "Value cannot be less than zero.");

                this.items = items.ToList();
                this.Page = page;
                this.PageSize = pageSize;
                this.TotalItems = totalItems;
            }

            public override string ToString()
            {
                return String.Format("{0} Item(s)", TotalItems);
            }
        }
#endregion
        private readonly string _classname = TestContext.CurrentContext.Test.ClassName + "+";
        [Test]
        public void Case5_Should_FindMessageTypes()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            //When
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(/*"MicroDocum.Analyzers.Tests.Case1+"*/_classname) ?? false);
            //Then
            Assert.That(c.Nodes, Has.Exactly(10).Items);
            Assert.That(c.Edges, Has.Count.AtLeast(6));
            Assert.That(c.GetSingles(), Has.Count.EqualTo(1), "GetSingles");
            Assert.That(c.GetHeads(), Has.Count.EqualTo(2), "GetHeads");
            Assert.That(c.GetLeafs(), Has.Count.EqualTo(2), "GetLeafs");
        }

        [Test]
        public void Case5_Should_SplitChains()
        {
            //Given
            var theme = new DefaultTheme();
            var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            var c = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(/*"MicroDocum.Analyzers.Tests.Case1+"*/_classname) ?? false);
            //When
            var chains = c.SplitChains();
            //Then
            Assert.That(chains, Has.Exactly(2).Items);
        }
    }
}
