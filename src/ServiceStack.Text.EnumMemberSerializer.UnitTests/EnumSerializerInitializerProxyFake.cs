using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class EnumSerializerInitializerProxyFake : IEnumSerializerInitializerProxy
    {
        public List<Type> ConfigedTypes = new List<Type>();

        public void ConfigEnumSerializers(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ConfigedTypes.Add(type);
        }

        public void ConfigEnumAndNullableEnumSerializers(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            ConfigedTypes.Add(type);
        }
    }
}