using System;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
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

                Assert.Equal(expectedSerializeFunc.Target, JsConfig<FakeTestingEnum>.SerializeFn.Target);
                Assert.Equal(expectedDeserializeFunc.Target, JsConfigFnTargetResolver<FakeTestingEnum>.GetDeserializerTarget());
            }
        }
    }
}