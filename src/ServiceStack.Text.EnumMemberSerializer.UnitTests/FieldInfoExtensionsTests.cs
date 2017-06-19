using System.Reflection;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class FieldInfoExtensionsTests
    {
        public class TestClass
        {
            public string MyFieldsName;
        }

        private const string MyFieldName = nameof(TestClass.MyFieldsName);
        private FieldInfo fieldInfo = typeof(TestClass).GetTypeInfo().GetField(MyFieldName);

        [Fact]
        public void MatchesDescription_NullFieldInfo_False()
        {
            FieldInfo nullFieldInfo = null;
            Assert.False(nullFieldInfo.MatchesDescription(MyFieldName));
        }
        
        [Fact]
        public void MatchesDescription_ConstantNameUsed_True()
        {
            Assert.True(fieldInfo.MatchesDescription(MyFieldName));
        }

        [Fact]
        public void MatchesDescription_NameCaseDiffers_True()
        {
            Assert.True(fieldInfo.MatchesDescription(MyFieldName.ToUpper()));
        }

        [Fact]
        public void MatchesDescription_WhiteSpacePadded_True()
        {
            Assert.True(fieldInfo.MatchesDescription("\t" + MyFieldName + "   \r"));
        }

        [Fact]
        public void MatchesDescription_FieldNameAndDescriptDiffer_False()
        {
            Assert.False(fieldInfo.MatchesDescription("won't match"));
        }
    }
}