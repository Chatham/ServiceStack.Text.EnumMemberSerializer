using System.Reflection;
using NSubstitute;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class FieldInfoExtensionsTests
    {
        private const string MyFieldName = "MyFieldsName";

        [Fact]
        public void MatchesDescription_ConstantNameUsed_True()
        {
            var fieldInfo = Substitute.For<FieldInfo>();
            fieldInfo.Name.Returns(MyFieldName);

            Assert.True(fieldInfo.MatchesDescription(MyFieldName));
        }

        [Fact]
        public void MatchesDescription_NameCaseDiffers_True()
        {
            var fieldInfo = Substitute.For<FieldInfo>();
            fieldInfo.Name.Returns(MyFieldName);

            Assert.True(fieldInfo.MatchesDescription(MyFieldName.ToUpper()));
        }

        [Fact]
        public void MatchesDescription_WhiteSpacePadded_True()
        {
            var fieldInfo = Substitute.For<FieldInfo>();
            fieldInfo.Name.Returns(MyFieldName);

            Assert.True(fieldInfo.MatchesDescription("\t" + MyFieldName + "   \r"));
        }

        [Fact]
        public void MatchesDescription_NullFieldNameNonNullDescript_False()
        {
            var fieldInfo = Substitute.For<FieldInfo>();
            fieldInfo.Name.Returns((string)null);

            Assert.False(fieldInfo.MatchesDescription(MyFieldName));
        }

        [Fact]
        public void MatchesDescription_EmptyFieldNameNonNullDescript_False()
        {
            var fieldInfo = Substitute.For<FieldInfo>();
            fieldInfo.Name.Returns(string.Empty);

            Assert.False(fieldInfo.MatchesDescription(MyFieldName));
        }

        [Fact]
        public void MatchesDescription_FieldNameAndDescriptDiffer_False()
        {
            var fieldInfo = Substitute.For<FieldInfo>();
            fieldInfo.Name.Returns("asdkjdhg");

            Assert.False(fieldInfo.MatchesDescription("won't match"));
        }
    }
}
