using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class CollectionExtensionsTests
    {
        [Fact]
        public void IsEmpty_NullList_True()
        {
            List<string> myList = null;
            Assert.True(myList.IsEmpty());
        }

        [Fact]
        public void IsEmpty_EmptyList_True()
        {
            var myList = new List<string>();
            Assert.True(myList.IsEmpty());
        }

        [Fact]
        public void IsEmpty_OneItemList_False()
        {
            var myList = new List<string> {"a value"};
            Assert.False(myList.IsEmpty());
        }
    }
}