using System.Reflection;
using Rhino.Mocks;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class FieldInfoExtensionsTests
    {
        private const string MyAssemblyName = "MyAssemblyName";

        [Fact]
        public void MatchesDescription_ConstantNameUsed_True()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(MyAssemblyName);

            Assert.True(fieldInfo.MatchesDescription(MyAssemblyName));
        }

        [Fact]
        public void MatchesDescription_NameCaseDiffers_True()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(MyAssemblyName);

            Assert.True(fieldInfo.MatchesDescription(MyAssemblyName.ToUpper()));
        }

        [Fact]
        public void MatchesDescription_NullFieldNameNonNullDescript_False()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(null);

            Assert.False(fieldInfo.MatchesDescription(MyAssemblyName));
        }

        [Fact]
        public void MatchesDescription_EmptyFieldNameNonNullDescript_False()
        {
            var fieldInfo = MockRepository.GenerateStub<FieldInfo>();
            fieldInfo.Stub(x => x.Name).Return(string.Empty);

            Assert.False(fieldInfo.MatchesDescription(MyAssemblyName));
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
