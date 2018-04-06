using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class PrettyEnumHelpersTests
    {
        private const string YourValueEnumMemberValue = "Your Value is worse";
        private readonly ITestOutputHelper _output;
        
        public PrettyEnumHelpersTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DeserializeEnum_WithCache_EnumsAddedToCache()
        {
            const int yourvalueInt = 123;
            const int myValueInt = 99;

            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();

            var yourvalueEnum =
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(yourvalueInt.ToString(), cache);
            var myvalueEnum = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(
                myValueInt.ToString(), cache);

            Assert.Equal(FakeTestingEnum.YourValue, yourvalueEnum);
            Assert.Equal(FakeTestingEnum.MyValue, myvalueEnum);
            Assert.Equal(2, cache.Count);
            Assert.True(cache.ContainsKey(yourvalueInt.ToString()));
            Assert.True(cache.ContainsKey(myValueInt.ToString()));
        }

        [Fact]
        public void DeserializeEnum_WithDefaultCache_EnumAddedToCache()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache =
                    new ConcurrentDictionary<string, FakeTestingEnum>();

                const int yourvalueInt = 123;

                var yourvalueEnum =
                    PrettyEnumHelpers<FakeTestingEnum>.GetEnumFrom(yourvalueInt.ToString());

                Assert.Equal(FakeTestingEnum.YourValue, yourvalueEnum);
                Assert.Single(PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache);
                Assert.True(
                    PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache.ContainsKey(yourvalueInt.ToString()));
            }
        }

        [Fact]
        public void DeserializeEnum_WithCacheAndNullString_DefaultEnumAndNothingAddedToCache()
        {
            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();
            var deserializedEnum = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(null, cache);

            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
            Assert.Empty(cache);
        }

        [Fact]
        public void DeserializeEnum_WithCacheAndEmptyString_DefaultEnumAndAddedToCache()
        {
            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();
            var deserializedEnum = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(
                string.Empty, cache);

            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
            Assert.Single(cache);
            Assert.True(cache.ContainsKey(string.Empty));
        }

        [Fact]
        public void DeserializeEnum_WithCacheAndWhiteSpaceString_DefaultEnumAndAddedToCache()
        {
            const string whitespace = "  \r  \t  ";

            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();
            var deserializedEnum = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(whitespace, cache);

            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
            Assert.Single(cache);
            Assert.True(cache.ContainsKey(whitespace));
        }

        [Fact]
        public void GetEnumFromDescription_FromInt_ReturnsCorrectEnum()
        {
            var deserializedEnum = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum("123");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_FromEnumName_ResturnsCorrectEnum()
        {
            var deserializedEnum = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum("YourValue");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_InvalidEnumString_ReturnsDefaultEnum()
        {
            var deserializedEnum =
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum("Invalid Value");
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_FromEnumNameMixedCase_ResturnsCorrectEnum()
        {
            var deserializedEnum = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(
                "YoURvaLuE");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_WhitespacePaddedEnum_ResturnsCorrectEnum()
        {
            var deserializedEnum =
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum("\tYourValue  ");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_FromEnumMemberAttributeName_ReturnsCorrectEnum()
        {
            var deserializedEnum =
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(YourValueEnumMemberValue);
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_WhiteSpacePaddedEnumMemberAttrName_ReturnsCorrectEnum()
        {
            var deserializedEnum =
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum("\t" + YourValueEnumMemberValue + "  ");
            Assert.Equal(FakeTestingEnum.YourValue, deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_NonEnumType_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                PrettyEnumHelpers<DateTime>.DeserializeEnum("yup"));
        }

        [Fact]
        public void GetEnumFromDescription_InvalidStringValue_ReturnsDefaultEnumValue()
        {
            FakeTestingEnum deserializedEnum =
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum("not valid description");
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_EmptyStringValue_ReturnsDefaultEnumValue()
        {
            FakeTestingEnum deserializedEnum =
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(string.Empty);
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void GetEnumFromDescription_NullStringValue_ReturnsDefaultEnumValue()
        {
            var deserializedEnum = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum(null);
            Assert.Equal(default(FakeTestingEnum), deserializedEnum);
        }

        [Fact]
        public void SerializeEnum_WithCacheAndValidDescription_CorrectEnumAddedToCache()
        {
            var cache = new ConcurrentDictionary<FakeTestingEnum, string>();
            var serializedEnum = PrettyEnumHelpers<FakeTestingEnum>.SerializeEnum(
                FakeTestingEnum.YourValue, cache);

            Assert.Equal(YourValueEnumMemberValue, serializedEnum);
            Assert.Single(cache);
            Assert.True(cache.ContainsKey(FakeTestingEnum.YourValue));
        }

        [Fact]
        public void SerializeEnum_DefaultCache_CorrectEnumAddedToCache()
        {
            lock (StaticTestingLocks.SerializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.SerializeCache =
                    new ConcurrentDictionary<FakeTestingEnum, string>();
                var serializedEnum = PrettyEnumHelpers<FakeTestingEnum>.GetOptimalEnumDescription(FakeTestingEnum.YourValue);

                Assert.Equal(YourValueEnumMemberValue, serializedEnum);
                Assert.Single(PrettyEnumHelpers<FakeTestingEnum>.SerializeCache);
                Assert.True(
                    PrettyEnumHelpers<FakeTestingEnum>.SerializeCache.ContainsKey(FakeTestingEnum.YourValue));
            }
        }

        [Fact]
        public void GetDescriptionFromEnum_NonEnumType_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                PrettyEnumHelpers<DateTime>.SerializeEnum(new DateTime()));
        }

        [Fact]
        public void GetDescriptionFromEnum_ValidEnumTypeWithAttr_ReturnsAttrString()
        {
            var enumString = PrettyEnumHelpers<FakeTestingEnum>.SerializeEnum(FakeTestingEnum.YourValue);
            Assert.Equal(YourValueEnumMemberValue, enumString);
        }


        [Fact(Skip = "For Reference")]
        public void SerializeEnum_ValidEnums_HelperMethodFasterThanToString()
        {
            var cache = new ConcurrentDictionary<FakeTestingEnum, string>();

            const int testIterations = 1000000;

            var _ = PrettyEnumHelpers<FakeTestingEnum>.SerializeEnum(FakeTestingEnum.YourValue, cache);
            var swSerEnumMember = new Stopwatch();
            swSerEnumMember.Start();


            for (var i = 0; i < testIterations; i++)
            {
                _ = PrettyEnumHelpers<FakeTestingEnum>.SerializeEnum(FakeTestingEnum.YourValue, cache);
            }

            swSerEnumMember.Stop();

            _ = FakeTestingEnum.YourValue.ToString();

            var swSerEnumToString = new Stopwatch();
            swSerEnumToString.Start();

            for (var i = 0; i < testIterations; i++)
            {
                _ = FakeTestingEnum.YourValue.ToString();
            }

            swSerEnumToString.Stop();

            _output.WriteLine($"Helper: {swSerEnumMember.Elapsed}");
            _output.WriteLine($"ToString: {swSerEnumToString.Elapsed}");
            _output.WriteLine($"Ratio: {swSerEnumToString.Elapsed.TotalMilliseconds / swSerEnumMember.Elapsed.TotalMilliseconds}");

            Assert.True(swSerEnumMember.Elapsed <= swSerEnumToString.Elapsed);
        }

        [Fact(Skip = "For Reference")]
        public void DeserializeEnum_ValidEnums_HelperMethodFasterThanEnumMethod()
        {
            var cache = new ConcurrentDictionary<string, FakeTestingEnum>();

            const int testIterations = 1000000;

            var _ = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum("YourValue", cache);

            var swDeserializeEnumMember = new Stopwatch();
            swDeserializeEnumMember.Start();

            for (var i = 0; i < testIterations; i++)
            {
                _ = PrettyEnumHelpers<FakeTestingEnum>.DeserializeEnum("YourValue", cache);
            }

            swDeserializeEnumMember.Stop();

            _ = (FakeTestingEnum)Enum.Parse(typeof(FakeTestingEnum), "YourValue");

            var swDeserializeToString = new Stopwatch();
            swDeserializeToString.Start();

            for (var i = 0; i < testIterations; i++)
            {
                _ = (FakeTestingEnum)Enum.Parse(typeof(FakeTestingEnum), "YourValue");
            }

            swDeserializeToString.Stop();

            _output.WriteLine($"Helper: {swDeserializeEnumMember.Elapsed}");
            _output.WriteLine($"ToString: {swDeserializeToString.Elapsed}");
            _output.WriteLine($"Ratio: {swDeserializeToString.Elapsed.TotalMilliseconds / swDeserializeEnumMember.Elapsed.TotalMilliseconds}");

            Assert.True(swDeserializeEnumMember.Elapsed <= swDeserializeToString.Elapsed);
        }

        [Fact]
        public void GetDescriptionFromEnum_ValidEnumTypeWithoutAttr_ReturnsEnumString()
        {
            var enumString = PrettyEnumHelpers<FakeTestingEnum>.SerializeEnum(FakeTestingEnum.MyValue);
            Assert.Equal("MyValue", enumString);
        }

        [Fact]
        public void GetDescriptionFromEnum_InValidEnumValue_ReturnsIntValueAsString()
        {
            var enumString =
                PrettyEnumHelpers<FakeTestingEnum>.SerializeEnum((FakeTestingEnum)int.MaxValue);
            Assert.Equal(int.MaxValue.ToString(), enumString);
        }
    }
}