using System;
using System.Reflection;
using SomeOtherNamespace;
using Xunit;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    public class EnumSerializerConfiguratorTests
    {
        [Fact]
        public void Configure_TestAssembly_JsConfigFuncsSet()
        {
            //Inspecting static values, so locking in cases tests are multi threaded.
            lock (StaticTestingLocks.JsConfigLockObject)
            {
                JsConfig<FakeTestingEnum>.Reset();
                JsConfig<FakeTestingEnum?>.Reset();

                new EnumSerializerConfigurator()
                    .WithAssemblies(new[] {typeof(FakeTestingEnum).GetTypeInfo().Assembly})
                    .WithNullableEnumSerializers()
                    .Configure();

                Func<FakeTestingEnum, string> expectedSerializeFunc =
                    PrettyEnumHelpers<FakeTestingEnum>.GetOptimalEnumDescription;
                Func<string, FakeTestingEnum> expectedDeserializeFunc =
                    PrettyEnumHelpers<FakeTestingEnum>.GetEnumFrom;

                Func<FakeTestingEnum?, string> expectedNullableSerializeFunc =
                    PrettyEnumHelpers<FakeTestingEnum>.GetOptimalEnumDescription;
                Func<string, FakeTestingEnum?> expectedNullableDeserializeFunc =
                    PrettyEnumHelpers<FakeTestingEnum>.GetNullableEnumFrom;

                Assert.Equal(expectedSerializeFunc, JsConfig<FakeTestingEnum>.SerializeFn);
                Assert.Equal(expectedDeserializeFunc.Target, JsConfigFnTargetResolver<FakeTestingEnum>.GetDeserializerTarget());
                Assert.Equal(expectedNullableSerializeFunc, JsConfig<FakeTestingEnum?>.SerializeFn);
                Assert.Equal(expectedNullableDeserializeFunc.Target, JsConfigFnTargetResolver<FakeTestingEnum?>.GetDeserializerTarget());
            }
        }

        [Fact]
        public void Configure_TestAssembly_AllEnumsConfigured()
        {
            var proxyFake = new EnumSerializerInitializerProxyFake();

            new EnumSerializerConfigurator {JsConfigProxy = proxyFake}
                .WithAssemblies(new[] { typeof(WhereTheEnumHasNoNamespace).GetTypeInfo().Assembly})
                .Configure();

            Assert.Equal(3, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof (WhereTheEnumHasNoNamespace)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof (FakeTestingEnum)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof (DifferentNamespaceEnum)));
        }

        [Fact]
        public void Configure_TestEnumsAddedIndividually_BothEnumsConfigured()
        {
            var proxyFake = new EnumSerializerInitializerProxyFake();

            new EnumSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithEnumTypes(new Type[]{null})
                .WithEnumTypes(new[] { typeof(FakeTestingEnum) })
                .WithEnumTypes(new[] { typeof(DifferentNamespaceEnum) })
                .Configure();

            Assert.Equal(2, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(FakeTestingEnum)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DifferentNamespaceEnum)));
        }

        [Fact]
        public void Configure_TestEnumsAddedTogether_BothEnumsConfigured()
        {
            var proxyFake = new EnumSerializerInitializerProxyFake();

            new EnumSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithEnumTypes(new Type[] { null })
                .WithEnumTypes(new[] { typeof(FakeTestingEnum), typeof(DifferentNamespaceEnum) })
                .Configure();

            Assert.Equal(2, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(FakeTestingEnum)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DifferentNamespaceEnum)));
        }

        [Fact]
        public void Configure_NamespaceExcludedEnumAddedExplicitly_AllEnumsConfigured()
        {
            var proxyFake = new EnumSerializerInitializerProxyFake();

            new EnumSerializerConfigurator {JsConfigProxy = proxyFake}
                .WithEnumTypes(new[] {typeof(DateTimeKind)})
                .WithEnumTypes(new[] {typeof(FakeTestingEnum)})
                .WithAssemblies(new[] {typeof(FakeTestingEnum).GetTypeInfo().Assembly})
                .WithNamespaceFilter(ns => ns.StartsWith("SomeOtherNamespace"))
                .Configure();

            Assert.Equal(3, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(FakeTestingEnum)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DifferentNamespaceEnum)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DateTimeKind)));
        }

        [Fact]
        public void Configure_NamespaceFilteredAndExplicitEnum_BothEnumsConfigured()
        {
            var proxyFake = new EnumSerializerInitializerProxyFake();

            new EnumSerializerConfigurator {JsConfigProxy = proxyFake}
                .WithEnumTypes(new[] {typeof(DateTimeKind)})
                .WithNamespaceFilter(ns => ns.StartsWith("SomeOtherNamespace"))
                .WithAssemblies(new[] {typeof(DifferentNamespaceEnum).GetTypeInfo().Assembly})
                .Configure();

            Assert.Equal(2, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DifferentNamespaceEnum)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DateTimeKind)));
        }

        [Fact]
        public void Configure_FilterSpecifiedNoAssembly_OnlyExplicitEnumsConfigured()
        {
            var proxyFake = new EnumSerializerInitializerProxyFake();

            new EnumSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithEnumTypes(new[] { typeof(DateTimeKind) })
                .WithNamespaceFilter(ns => ns.StartsWith("SomeOtherNamespace"))
                .Configure();

            Assert.Equal(1, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DateTimeKind)));
        }
    }
}