using System;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerInitializerProxy : IEnumSerializerInitializerProxy
    {
        //Hide the static class interaction as much as possible
        public void ConfigEnumSerializers(Type type)
        {
            Type enumHelperType = typeof(EnumSerializerInitializer<>).MakeGenericType(new[] { type });
            enumHelperType.CreateInstance();
        }
    }
}