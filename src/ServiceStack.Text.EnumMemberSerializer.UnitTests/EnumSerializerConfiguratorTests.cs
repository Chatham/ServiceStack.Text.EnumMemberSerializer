using System;
using System.Collections.Generic;
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
            lock (JsConfigLock.LockObject)
            {
                JsConfig<FakeTestingEnum>.Reset();

                new EnumSerializerConfigurator()
                    .WithAssemblies(new[] {Assembly.GetExecutingAssembly()})
                    .Configure();

                Func<FakeTestingEnum, string> expectedSerializeFunc =
                    EnumSerializerHelpers<FakeTestingEnum>.SerializeEnum;
                Func<string, FakeTestingEnum> expectedDeserializeFunc =
                    EnumSerializerHelpers<FakeTestingEnum>.DeserializeEnum;

                Assert.Equal(expectedSerializeFunc, JsConfig<FakeTestingEnum>.SerializeFn);
                Assert.Equal(expectedDeserializeFunc, JsConfig<FakeTestingEnum>.DeSerializeFn);
            }
        }

        [Fact]
        public void Configure_TestAssembly_BothEnumsConfigured()
        {
            var proxyFake = new EnumSerializerHelpersProxyFake();

            new EnumSerializerConfigurator {JsConfigProxy = proxyFake}
                .WithAssemblies(new[] {Assembly.GetExecutingAssembly()})
                .Configure();

            Assert.Equal(2, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof (FakeTestingEnum)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof (DifferentNamespaceEnum)));
        }

        [Fact]
        public void Configure_TestEnumsAddedIndividually_BothEnumsConfigured()
        {
            var proxyFake = new EnumSerializerHelpersProxyFake();

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
            var proxyFake = new EnumSerializerHelpersProxyFake();

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
            var proxyFake = new EnumSerializerHelpersProxyFake();

            new EnumSerializerConfigurator {JsConfigProxy = proxyFake}
                .WithEnumTypes(new[] {typeof (DateTimeKind)})
                .WithEnumTypes(new[] {typeof (FakeTestingEnum)})
                .WithAssemblies(new[] {Assembly.GetExecutingAssembly()})
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
            var proxyFake = new EnumSerializerHelpersProxyFake();

            new EnumSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithEnumTypes(new[] { typeof(DateTimeKind) })
                .WithNamespaceFilter(ns => ns.StartsWith("SomeOtherNamespace"))
                .WithAssemblies(new[] { Assembly.GetExecutingAssembly() })
                .Configure();

            Assert.Equal(2, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DifferentNamespaceEnum)));
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DateTimeKind)));
        }

        [Fact]
        public void Configure_FilterSpecifiedNoAssembly_OnlyExplicitEnumsConfigured()
        {
            var proxyFake = new EnumSerializerHelpersProxyFake();

            new EnumSerializerConfigurator { JsConfigProxy = proxyFake }
                .WithEnumTypes(new[] { typeof(DateTimeKind) })
                .WithNamespaceFilter(ns => ns.StartsWith("SomeOtherNamespace"))
                .Configure();

            Assert.Equal(1, proxyFake.ConfigedTypes.Count);
            Assert.True(proxyFake.ConfigedTypes.Contains(typeof(DateTimeKind)));
        }
    }
}