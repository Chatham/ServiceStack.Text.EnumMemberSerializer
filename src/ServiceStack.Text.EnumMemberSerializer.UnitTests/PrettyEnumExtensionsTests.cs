using System;
using System.Collections.Concurrent;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    //These test are highly duplicative, only doing a small amount.
    public class PrettyEnumExtensionsTests
    {
        [Fact]
        public void GetOptimalEnumDescription_ValidEnum_ReturnsPrettyDescription()
        {
            lock (StaticTestingLocks.SerializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.SerializeCache = new ConcurrentDictionary<FakeTestingEnum, string>();

                const FakeTestingEnum testEnum = FakeTestingEnum.YourValue;
                var actualDescription = testEnum.GetOptimalEnumDescription();
                Assert.Equal("Your Value is worse", actualDescription);
            }
        }

        [Fact]
        public void GetOptimalEnumDescription_NullEnum_ReturnsNull()
        {
            lock (StaticTestingLocks.SerializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.SerializeCache = new ConcurrentDictionary<FakeTestingEnum, string>();
                FakeTestingEnum? testEnum = null;

                Assert.Null(testEnum.GetOptimalEnumDescription());
            }
        }

        [Fact]
        public void GetOptimalEnumDescription_NullableEnumWithValue_ReturnsPrettyDescription()
        {
            lock (StaticTestingLocks.SerializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.SerializeCache = new ConcurrentDictionary<FakeTestingEnum, string>();
                FakeTestingEnum? testEnum = FakeTestingEnum.YourValue;

                Assert.Equal("Your Value is worse", testEnum.GetOptimalEnumDescription());
            }
        }

        [Fact]
        public void GetOptimalEnumDescription_NonEnum_ThrowsException()
        {
            lock (StaticTestingLocks.SerializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.SerializeCache = new ConcurrentDictionary<FakeTestingEnum, string>();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    const int someNumber = 0;
                    var _ = someNumber.GetOptimalEnumDescription();
                });
            }
        }

        [Fact]
        public void GetEnum_PrettyDescription_ReturnsEnum()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache =
                    new ConcurrentDictionary<string, FakeTestingEnum>();
                const string prettyDescription = "Your Value is worse";
                var actualEnum = prettyDescription.GetEnum<FakeTestingEnum>();
                Assert.Equal(FakeTestingEnum.YourValue, actualEnum);
            }
        }

        [Fact]
        public void GetEnum_PrettyDescriptionWithCommas_ReturnsEnum()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache =
                    new ConcurrentDictionary<string, FakeTestingEnum>();
                const string prettyDescription = "Commas, Problems";
                var actualEnum = prettyDescription.GetEnum<FakeTestingEnum>();
                Assert.Equal(FakeTestingEnum.CommasProblems, actualEnum);
            }
        }

        [Fact]
        public void GetEnum_InvalidDescription_ReturnsDefaultEnum()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                PrettyEnumHelpers<DateTimeKind>.DeserializeCache = new ConcurrentDictionary<string, DateTimeKind>();
                var prettyDescription = "Your Value is worse";
                var actualEnum = prettyDescription.GetEnum<DateTimeKind>();
                Assert.Equal(default(DateTimeKind), actualEnum);
            }
        }

        [Fact]
        public void GetEnum_NullString_ReturnsDefaultEnum()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache =
                    new ConcurrentDictionary<string, FakeTestingEnum>();
                string prettyDescription = null;
                var actualEnum = prettyDescription.GetEnum<FakeTestingEnum>();
                Assert.Equal(default(FakeTestingEnum), actualEnum);
            }
        }

        [Fact]
        public void GetNullableEnum_NullValue_ReturnsNull()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache =
                    new ConcurrentDictionary<string, FakeTestingEnum>();
                string prettyDescription = null;
                var actualEnum = prettyDescription.GetNullableEnum<FakeTestingEnum>();
                Assert.Null(actualEnum);
            }
        }

        [Fact]
        public void GetNullableEnum_EmptyValue_ReturnsDefaultEnum()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache =
                    new ConcurrentDictionary<string, FakeTestingEnum>();
                var prettyDescription = string.Empty;
                var actualEnum = prettyDescription.GetNullableEnum<FakeTestingEnum>();
                Assert.Equal(default(FakeTestingEnum), actualEnum);
            }
        }

        [Fact]
        public void GetNullableEnum_WhitespaceValue_ReturnsDefaultEnum()
        {
            lock (StaticTestingLocks.DeserializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache =
                    new ConcurrentDictionary<string, FakeTestingEnum>();
                const string prettyDescription = "\t   \n  \r   ";
                var actualEnum = prettyDescription.GetNullableEnum<FakeTestingEnum>();
                Assert.Equal(default(FakeTestingEnum), actualEnum);
            }
        }
    }
}