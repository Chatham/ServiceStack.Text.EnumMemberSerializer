using System;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class EnumSerializerInitializerTests
    {
        [Fact]
        public void Constructor_NonEnumStruct_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new EnumSerializerInitializer<DateTime>());
        }        
    }
}