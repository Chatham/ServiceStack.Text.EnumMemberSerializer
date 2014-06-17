using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Rhino.Mocks;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class FieldInfoExtensionsTests
    {
        private const string MyFieldName = "MyFieldsName";

        [Fact]
        public void MatchesDescription_ConstantNameUsed_True()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(MyFieldName);

            Assert.True(fieldInfo.MatchesDescription(MyFieldName));
        }

        [Fact]
        public void MatchesDescription_NameCaseDiffers_True()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(MyFieldName);

            Assert.True(fieldInfo.MatchesDescription(MyFieldName.ToUpper()));
        }

        [Fact]
        public void MatchesDescription_WhiteSpacePadded_True()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(MyFieldName);

            Assert.True(fieldInfo.MatchesDescription("\t" + MyFieldName + "   \r"));
        }

        [Fact]
        public void MatchesDescription_NullFieldNameNonNullDescript_False()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(null);

            Assert.False(fieldInfo.MatchesDescription(MyFieldName));
        }

        [Fact]
        public void MatchesDescription_EmptyFieldNameNonNullDescript_False()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(string.Empty);

            Assert.False(fieldInfo.MatchesDescription(MyFieldName));
        }

        [Fact]
        public void MatchesDescription_FieldNameAndDescriptDiffer_False()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return("asdkjdhg");

            Assert.False(fieldInfo.MatchesDescription("won't match"));
        }
    }
}
