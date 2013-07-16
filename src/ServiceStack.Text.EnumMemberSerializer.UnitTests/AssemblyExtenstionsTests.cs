using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SomeOtherNamespace;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class AssemblyExtenstionsTests
    {
        [Fact]
        public void GetPublicEnums_UnitTestClassNoFilter_ReturnsAllEnums()
        {
            var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly()};
            var publicEnums = assemblies.GetPublicEnums(EnumSerializerConfigurator.AlwaysTrueFilter);

            Assert.Equal(3, publicEnums.Count);
            Assert.True(publicEnums.Contains(typeof (WhereTheEnumHasNoNamespace)));
            Assert.True(publicEnums.Contains(typeof (FakeTestingEnum)));
            Assert.True(publicEnums.Contains(typeof (DifferentNamespaceEnum)));
        }

        [Fact]
        public void GetPublicEnums_UnitTestClassWithFilter_ReturnsDifferentNamespaceEnum()
        {
            var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly()};
            var publicEnums = assemblies.GetPublicEnums(s => s.StartsWith("SomeOtherNamespace"));

            Assert.Equal(1, publicEnums.Count);
            Assert.Equal(typeof (DifferentNamespaceEnum), publicEnums.First());
        }

        [Fact]
        public void GetPublicEnums_DuplicateAssemblyInList_ReturnsAllDistinctEnums()
        {
            var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly(), Assembly.GetExecutingAssembly()};
            var publicEnums = assemblies.GetPublicEnums(EnumSerializerConfigurator.AlwaysTrueFilter);

            Assert.Equal(3, publicEnums.Count);
            Assert.True(publicEnums.Contains(typeof (WhereTheEnumHasNoNamespace)));
            Assert.True(publicEnums.Contains(typeof (FakeTestingEnum)));
            Assert.True(publicEnums.Contains(typeof (DifferentNamespaceEnum)));
        }

        [Fact]
        public void GetPublicEnums_EmptyAssemblyList_EmptyTypes()
        {
            var emptyAssemblyList = new List<Assembly>();
            var publicEnums = emptyAssemblyList.GetPublicEnums(EnumSerializerConfigurator.AlwaysTrueFilter);
            Assert.Equal(0, publicEnums.Count);
        }

        [Fact]
        public void GetPublicEnums_NullAssemblyList_EmptyTypes()
        {
            List<Assembly> emptyAssemblyList = null;
            var publicEnums = emptyAssemblyList.GetPublicEnums(EnumSerializerConfigurator.AlwaysTrueFilter);
            Assert.Equal(0, publicEnums.Count);
        }

        [Fact]
        public void GetPublicEnums_UnitTestClassNullFilter_ReturnsAllEnums()
        {
            var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly()};
            var publicEnums = assemblies.GetPublicEnums(null);

            Assert.Equal(3, publicEnums.Count);
            Assert.True(publicEnums.Contains(typeof (WhereTheEnumHasNoNamespace)));
            Assert.True(publicEnums.Contains(typeof (FakeTestingEnum)));
            Assert.True(publicEnums.Contains(typeof (DifferentNamespaceEnum)));
        }

        [Fact]
        public void GetPublicEnums_UnitTestClassWithAlwaysFalseFilter_ReturnsEmptyTypeList()
        {
            var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly()};
            var publicEnums = assemblies.GetPublicEnums(s => false);

            Assert.Equal(0, publicEnums.Count);
        }

        [Fact]
        public void GetPublicEnums_ListOfNullAssemblies_ReturnsEmtpyTypeList()
        {
            var assemblies = new List<Assembly> {null};
            var publicEnums = assemblies.GetPublicEnums(EnumSerializerConfigurator.AlwaysTrueFilter);

            Assert.Equal(0, publicEnums.Count);
        }

        [Fact]
        public void GetPublicEnums_FilterThrowsException_ExceptionPassedUp()
        {
            var assemblies = new List<Assembly> {Assembly.GetExecutingAssembly()};
            Assert.Throws<NotImplementedException>(
                () =>
                {
                    assemblies.GetPublicEnums(s => { throw new NotImplementedException(); });
                });
        }

        [Fact]
        public void GetPublicEnums_CheckNoNullStrings_NullNamespaceEnumIsNotNullString()
        {
            var assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
            assemblies.GetPublicEnums(
                s =>
                {
                    Assert.NotNull(s);
                    return true;
                });
        }
    }
}