using System;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class EnumSerializerHelpersProxyTests
    {
        [Fact]
        public void ConfigEnumSerializers_EnumType_JsConfigFuncsSet()
        {
            //Inspecting static values, so locking in cases tests are multi threaded.
            lock (StaticTestingLocks.JsConfigLockObject)
            {
                JsConfig<FakeTestingEnum>.Reset();

                //Testing static class is fun
                var proxy = new EnumSerializerHelpersProxy();
                proxy.ConfigEnumSerializers(typeof (FakeTestingEnum));
                //new EnumSerializerHelpers<FakeTestingEnum>();
                Func<FakeTestingEnum, string> expectedSerializeFunc =
                    EnumSerializerHelpers<FakeTestingEnum>.SerializeEnum;
                Func<string, FakeTestingEnum> expectedDeserializeFunc =
                    EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum;

                Assert.Equal(expectedSerializeFunc, JsConfig<FakeTestingEnum>.SerializeFn);
                Assert.Equal(expectedDeserializeFunc, JsConfig<FakeTestingEnum>.DeSerializeFn);
            }
        }
    }
}