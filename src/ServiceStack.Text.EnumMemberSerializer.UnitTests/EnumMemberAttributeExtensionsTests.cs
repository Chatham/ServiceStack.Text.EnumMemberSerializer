using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class EnumMemberAttributeExtensionsTests
    {
        private const string MyEnumMemberValue = "MyEnumMemberValue";

        [Fact]
        public void MatchesDescription_ConstantNameUsed_True()
        {
            var attribute = new EnumMemberAttribute {Value = MyEnumMemberValue};
            Assert.True(attribute.MatchesDescription(MyEnumMemberValue));
        }
        
        [Fact]
        public void MatchesDescription_NameCaseDiffers_True()
        {
            var attribute = new EnumMemberAttribute { Value = MyEnumMemberValue };
            Assert.True(attribute.MatchesDescription(MyEnumMemberValue.ToUpper()));
        }

        [Fact]
        public void MatchesDescription_WhiteSpacePaddedConstantNameUsed_True()
        {
            var attribute = new EnumMemberAttribute { Value = MyEnumMemberValue };
            Assert.True(attribute.MatchesDescription("  " + MyEnumMemberValue + "\t\r\t  "));
        }

        [Fact]
        public void MatchesDescription_NullFieldNameNonNullDescript_False()
        {
            var attribute = new EnumMemberAttribute { Value = null };
            Assert.False(attribute.MatchesDescription(MyEnumMemberValue));
        }

        [Fact]
        public void MatchesDescription_EmptyFieldNameNonNullDescript_False()
        {
            var attribute = new EnumMemberAttribute { Value = string.Empty };
            Assert.False(attribute.MatchesDescription(MyEnumMemberValue));
        }

        [Fact]
        public void MatchesDescription_FieldNameAndDescriptDiffer_False()
        {
            var attribute = new EnumMemberAttribute { Value = "bad value" };
            Assert.False(attribute.MatchesDescription("won't match"));
        }

        [Fact]
        public void MatchesDescription_NullAttribute_False()
        {
            EnumMemberAttribute attribute = null;
            Assert.False(attribute.MatchesDescription("won't match"));
        }
    }
}
