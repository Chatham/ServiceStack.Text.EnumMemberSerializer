using System;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class EnumSerializerHelpersTests
    {
        [Fact]
        public void DeserializeEnum_FromInt_ReturnsCorrectEnum()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("123");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void DeserializeEnum_FromEnumName_ResturnsCorrectEnum()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("YourValue");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void DeserializeEnum_InvalidEnumString_ReturnsDefaultEnum()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("Invalid Value");
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void DeserializeEnum_FromEnumNameMixedCase_ResturnsCorrectEnum()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("YoURvaLuE");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void DeserializeEnum_WhitespacePaddedEnum_ResturnsCorrectEnum()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("\tYourValue  ");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void DeserializeEnum_FromEnumMemberAttributeName_ReturnsCorrectEnum()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("Your Value is worse");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void DeserializeEnum_WhiteSpacePaddedEnumMemberAttrName_ReturnsCorrectEnum()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("\tYour Value is worse  ");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }


        [Fact]
        public void DeserializeEnum_NonEnumType_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                EnumSerializerHelpers<DateTime>.DeserializeEnum("yup"));
        }

        [Fact]
        public void DeserializeEnum_InvalidStringValue_ReturnsDefaultEnumValue()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("not valid description");
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void DeserializeEnum_EmptyStringValue_ReturnsDefaultEnumValue()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum(string.Empty);
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void DeserializeEnum_NullStringValue_ReturnsDefaultEnumValue()
        {
            var deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum(null);
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void SerializeEnum_NonEnumType_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                EnumSerializerHelpers<DateTime>.SerializeEnum(new DateTime()));
        }

        [Fact]
        public void SerializeEnum_ValidEnumTypeWithAttr_ReturnsAttrString()
        {
            var enumString = EnumSerializerHelpers<FakeTestingEnum>.SerializeEnum(FakeTestingEnum.YourValue);
            Assert.Equal("Your Value is worse", enumString);
        }

        [Fact]
        public void SerializeEnum_ValidEnumTypeWithoutAttr_ReturnsEnumString()
        {
            var enumString = EnumSerializerHelpers<FakeTestingEnum>.SerializeEnum(FakeTestingEnum.MyValue);
            Assert.Equal("MyValue", enumString);
        }

        [Fact]
        public void SerializeEnum_InValidEnumValue_ReturnsIntValueAsString()
        {
            var enumString = EnumSerializerHelpers<FakeTestingEnum>.SerializeEnum((FakeTestingEnum)int.MaxValue);
            Assert.Equal(int.MaxValue.ToString(), enumString);
        }

        [Fact]
        public void Constructor_NonEnumStruct_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new EnumSerializerHelpers<DateTime>());
        }
    }
}
