using System;
using System.Collections.Generic;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
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
    }
}