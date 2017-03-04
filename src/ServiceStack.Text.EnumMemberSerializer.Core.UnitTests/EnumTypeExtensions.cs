using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class EnumTypeExtensions
    {
        [Fact]
        public void GetPublicEnums_NullTypeList_EmptyEnumList()
        {
            List<Type> nullTypeList = null;
            var publicEnums = nullTypeList.GetPublicEnums();
            Assert.Equal(0, publicEnums.Count);
        }

        [Fact]
        public void GetPublicEnums_EmptyTypeList_EmptyEnumList()
        {
            var typeList = new List<Type>();
            var publicEnums = typeList.GetPublicEnums();
            Assert.Equal(0, publicEnums.Count);
        }

        [Fact]
        public void GetPublicEnums_NonPublicEnum_EmptyEnumList()
        {
            var typeList = new List<Type> { typeof(NonPublicEnum) };
            var publicEnums = typeList.GetPublicEnums();
            Assert.Equal(0, publicEnums.Count);
        }

        [Fact]
        public void GetPublicEnums_MixedPublicNonPublicEnums_OnlyPublicEnumsInList()
        {
            var typeList = new List<Type> { typeof(NonPublicEnum), typeof(FakeTestingEnum) };
            var publicEnums = typeList.GetPublicEnums();
            Assert.Equal(1, publicEnums.Count);
            Assert.Equal(typeof(FakeTestingEnum), publicEnums.First());
        }

        [Fact]
        public void GetPublicEnums_NonEnumTypeList_EmptyTypeList()
        {
            var typeList = new List<Type> { typeof(string), typeof(DateTime) };
            var publicEnums = typeList.GetPublicEnums();
            Assert.Equal(0, publicEnums.Count);
        }

        [Fact]
        public void GetPublicEnums_ListOfNullTypes_EmptyEnumList()
        {
            var typeList = new List<Type> { null };
            var publicEnums = typeList.GetPublicEnums();
            Assert.Equal(0, publicEnums.Count);
        }
    }
}
