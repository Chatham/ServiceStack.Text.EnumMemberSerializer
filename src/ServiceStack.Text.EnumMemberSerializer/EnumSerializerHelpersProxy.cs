using System;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerHelpersProxy : IEnumSerializerHelpersProxy
    {
        //Hide the static class interaction as much as possible
        public void ConfigEnumSerializers(Type type)
        {
            Type enumHelperType = typeof(EnumSerializerHelpers<>).MakeGenericType(new[] { type });
            enumHelperType.CreateInstance();
        }
    }
}