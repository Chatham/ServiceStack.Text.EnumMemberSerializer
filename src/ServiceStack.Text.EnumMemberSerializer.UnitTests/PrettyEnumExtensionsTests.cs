using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                PrettyEnumHelpers<FakeTestingEnum>.SerializeCache =
                    new ConcurrentDictionary<FakeTestingEnum, string>();

                var testEnum = FakeTestingEnum.YourValue;
                var actualDescription = testEnum.GetOptimalEnumDescription();
                Assert.Equal("Your Value is worse", actualDescription);
            }
        }

        [Fact]
        public void GetOptimalEnumDescription_NonEnum_ThrowsException()
        {
            lock (StaticTestingLocks.SerializeCacheLockObject)
            {
                PrettyEnumHelpers<FakeTestingEnum>.SerializeCache =
                    new ConcurrentDictionary<FakeTestingEnum, string>();

                Assert.Throws<InvalidOperationException>(
                    () =>
                    {
                        int someNumber = 0;
                        var actualDescription = someNumber.GetOptimalEnumDescription();
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
                var prettyDescription = "Your Value is worse";
                var actualEnum = prettyDescription.GetEnum<FakeTestingEnum>();
                Assert.Equal(FakeTestingEnum.YourValue, actualEnum);
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
                PrettyEnumHelpers<FakeTestingEnum>.DeserializeCache = new ConcurrentDictionary<string, FakeTestingEnum>();
                string prettyDescription = null;
                var actualEnum = prettyDescription.GetEnum<FakeTestingEnum>();
                Assert.Equal(default(FakeTestingEnum), actualEnum);
            }
        }
    }
}
