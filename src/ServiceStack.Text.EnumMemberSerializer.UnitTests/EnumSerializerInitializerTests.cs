using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class EnumSerializerInitializerTests
    {
        [Fact]
        public void Constructor_NonEnumStruct_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new EnumSerializerInitializer<DateTime>());
        }        
    }
}