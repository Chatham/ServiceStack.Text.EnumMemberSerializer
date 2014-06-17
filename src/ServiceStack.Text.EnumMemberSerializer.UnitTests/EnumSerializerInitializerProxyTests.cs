using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class EnumSerializerInitializerProxyTests
    {
        [Fact]
        public void ConfigEnumSerializers_EnumType_JsConfigFuncsSet()
        {
            //Inspecting static values, so locking in cases tests are multi threaded.
            lock (StaticTestingLocks.JsConfigLockObject)
            {
                JsConfig<FakeTestingEnum>.Reset();

                //Testing static class is fun
                var proxy = new EnumSerializerInitializerProxy();
                proxy.ConfigEnumSerializers(typeof (FakeTestingEnum));

                Func<FakeTestingEnum, string> expectedSerializeFunc =
                    PrettyEnumHelpers<FakeTestingEnum>.GetOptimalEnumDescription;
                Func<string, FakeTestingEnum> expectedDeserializeFunc =
                    PrettyEnumHelpers<FakeTestingEnum>.GetEnumFrom;

                Assert.Equal(expectedSerializeFunc, JsConfig<FakeTestingEnum>.SerializeFn);
                Assert.Equal(expectedDeserializeFunc, JsConfig<FakeTestingEnum>.DeSerializeFn);
            }
        }
    }
}