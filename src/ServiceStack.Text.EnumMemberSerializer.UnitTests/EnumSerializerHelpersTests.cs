using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class EnumSerializerHelpersTests
    {
        private const string YourValueEnumMemberValue = "Your Value is worse";

        [Fact]
        public void DeserializeEnum_WithCache_EnumsAddedToCache()
        {
            const int YourvalueInt = 123;
            const int MyValueInt = 99;

            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();

            FakeTestingEnum yourvalueEnum =
                EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum(YourvalueInt.ToString(), cache);
            FakeTestingEnum myvalueEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum(
                MyValueInt.ToString(), cache);

            Assert.Equal(FakeTestingEnum.YourValue, yourvalueEnum);
            Assert.Equal(FakeTestingEnum.MyValue, myvalueEnum);
            Assert.Equal(2, cache.Count);
            Assert.True(cache.ContainsKey(YourvalueInt.ToString()));
            Assert.True(cache.ContainsKey(MyValueInt.ToString()));
        }

        [Fact]
        public void DeserializeEnum_WithDefaultCache_EnumAddedToCache()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                EnumSerializerHelpers<FakeTestingEnum>.DeserializeCache =
                    new ConcurrentDictionary<string, FakeTestingEnum>();

                const int YourvalueInt = 123;

                FakeTestingEnum yourvalueEnum =
                    EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum(YourvalueInt.ToString());

                Assert.Equal(FakeTestingEnum.YourValue, yourvalueEnum);
                Assert.Equal(1, EnumSerializerHelpers<FakeTestingEnum>.DeserializeCache.Count);
                Assert.True(
                    EnumSerializerHelpers<FakeTestingEnum>.DeserializeCache.ContainsKey(YourvalueInt.ToString()));
            }
        }

        [Fact]
        public void DeserializeEnum_WithCacheAndNullString_DefaultEnumAndNothingAddedToCache()
        {
            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();
            FakeTestingEnum deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum(null, cache);

            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
            Assert.Equal(0, cache.Count);
        }

        [Fact]
        public void DeserializeEnum_WithCacheAndEmptyString_DefaultEnumAndAddedToCache()
        {
            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();
            FakeTestingEnum deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum(
                string.Empty, cache);

            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
            Assert.Equal(1, cache.Count);
            Assert.True(cache.ContainsKey(string.Empty));
        }

        [Fact]
        public void DeserializeEnum_WithCacheAndWhiteSpaceString_DefaultEnumAndAddedToCache()
        {
            const string Whitespace = "  \r  \t  ";

            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();
            FakeTestingEnum deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum(Whitespace, cache);

            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
            Assert.Equal(1, cache.Count);
            Assert.True(cache.ContainsKey(Whitespace));
        }

        [Fact]
        public void GetEnumFromDescription_FromInt_ReturnsCorrectEnum()
        {
            FakeTestingEnum deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription("123");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_FromEnumName_ResturnsCorrectEnum()
        {
            FakeTestingEnum deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription(
                "YourValue");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_InvalidEnumString_ReturnsDefaultEnum()
        {
            FakeTestingEnum deserializedEnum =
                EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription("Invalid Value");
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_FromEnumNameMixedCase_ResturnsCorrectEnum()
        {
            FakeTestingEnum deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription(
                "YoURvaLuE");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_WhitespacePaddedEnum_ResturnsCorrectEnum()
        {
            FakeTestingEnum deserializedEnum =
                EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription("\tYourValue  ");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_FromEnumMemberAttributeName_ReturnsCorrectEnum()
        {
            FakeTestingEnum deserializedEnum =
                EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription(YourValueEnumMemberValue);
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_WhiteSpacePaddedEnumMemberAttrName_ReturnsCorrectEnum()
        {
            FakeTestingEnum deserializedEnum =
                EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription("\t" + YourValueEnumMemberValue + "  ");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_NonEnumType_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                EnumSerializerHelpers<DateTime>.GetEnumFromDescription("yup"));
        }

        [Fact]
        public void GetEnumFromDescription_InvalidStringValue_ReturnsDefaultEnumValue()
        {
            FakeTestingEnum deserializedEnum =
                EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription("not valid description");
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_EmptyStringValue_ReturnsDefaultEnumValue()
        {
            FakeTestingEnum deserializedEnum =
                EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription(string.Empty);
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_NullStringValue_ReturnsDefaultEnumValue()
        {
            FakeTestingEnum deserializedEnum = EnumSerializerHelpers<FakeTestingEnum>.GetEnumFromDescription(null);
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void SerializeEnum_WithCacheAndValidDescription_CorrectEnumAddedToCache()
        {
            var cache = new ConcurrentDictionary<FakeTestingEnum, string>();
            string serializedEnum = EnumSerializerHelpers<FakeTestingEnum>.SerializeEnum(
                FakeTestingEnum.YourValue, cache);

            Assert.Equal(YourValueEnumMemberValue, serializedEnum);
            Assert.Equal(1, cache.Count);
            Assert.True(cache.ContainsKey(FakeTestingEnum.YourValue));
        }

        [Fact]
        public void SerializeEnum_DefaultCache_CorrectEnumAddedToCache()
        {
            lock (StaticTestingLocks.SerializeCacheLockObject)
            {
                EnumSerializerHelpers<FakeTestingEnum>.SerializeCache =
                    new ConcurrentDictionary<FakeTestingEnum, string>();
                string serializedEnum = EnumSerializerHelpers<FakeTestingEnum>.SerializeEnum(FakeTestingEnum.YourValue);

                Assert.Equal(YourValueEnumMemberValue, serializedEnum);
                Assert.Equal(1, EnumSerializerHelpers<FakeTestingEnum>.SerializeCache.Count);
                Assert.True(
                    EnumSerializerHelpers<FakeTestingEnum>.SerializeCache.ContainsKey(FakeTestingEnum.YourValue));
            }
        }

        [Fact]
        public void GetDescriptionFromEnum_NonEnumType_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                EnumSerializerHelpers<DateTime>.GetDescriptionFromEnum(new DateTime()));
        }

        [Fact]
        public void GetDescriptionFromEnum_ValidEnumTypeWithAttr_ReturnsAttrString()
        {
            string enumString = EnumSerializerHelpers<FakeTestingEnum>.GetDescriptionFromEnum(FakeTestingEnum.YourValue);
            Assert.Equal(YourValueEnumMemberValue, enumString);
        }

        [Fact(Skip = "Performance Test For Reference")]
        public void SerializeEnum_ValidEnums_HelperMethodFasterThanToString()
        {
            var cache = new ConcurrentDictionary<FakeTestingEnum, string>();

            const int testIterations = 100000;

            var swSerEnumMember = new Stopwatch();
            swSerEnumMember.Start();

            for (int i = 0; i < testIterations; i++)
            {
                string value = EnumSerializerHelpers<FakeTestingEnum>.SerializeEnum(FakeTestingEnum.YourValue, cache);
            }

            swSerEnumMember.Stop();

            var swSerEnumToString = new Stopwatch();
            swSerEnumToString.Start();

            for (int i = 0; i < testIterations; i++)
            {
                string value = FakeTestingEnum.YourValue.ToString();
            }

            swSerEnumToString.Stop();

            Assert.True(swSerEnumMember.ElapsedTicks <= swSerEnumToString.ElapsedTicks);
        }

        [Fact(Skip = "Performance Test For Reference")]
        public void DeserializeEnum_ValidEnums_HelperMethodFasterThanEnumMethod()
        {
            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();

            const int testIterations = 100000;

            var swDeserializeEnumMember = new Stopwatch();
            swDeserializeEnumMember.Start();

            for (int i = 0; i < testIterations; i++)
            {
                FakeTestingEnum value = EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum("YourValue", cache);
            }

            swDeserializeEnumMember.Stop();

            var swDeserializeStringTo = new Stopwatch();
            swDeserializeStringTo.Start();

            for (int i = 0; i < testIterations; i++)
            {
                object value = Enum.Parse(typeof (FakeTestingEnum), "YourValue");
            }

            swDeserializeStringTo.Stop();
            Assert.True(swDeserializeEnumMember.ElapsedTicks <= swDeserializeStringTo.ElapsedTicks);
        }

        [Fact]
        public void GetDescriptionFromEnum_ValidEnumTypeWithoutAttr_ReturnsEnumString()
        {
            string enumString = EnumSerializerHelpers<FakeTestingEnum>.GetDescriptionFromEnum(FakeTestingEnum.MyValue);
            Assert.Equal("MyValue", enumString);
        }

        [Fact]
        public void GetDescriptionFromEnum_InValidEnumValue_ReturnsIntValueAsString()
        {
            string enumString =
                EnumSerializerHelpers<FakeTestingEnum>.GetDescriptionFromEnum((FakeTestingEnum) int.MaxValue);
            Assert.Equal(int.MaxValue.ToString(), enumString);
        }

        [Fact]
        public void Constructor_NonEnumStruct_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new EnumSerializerHelpers<DateTime>());
        }
    }
}