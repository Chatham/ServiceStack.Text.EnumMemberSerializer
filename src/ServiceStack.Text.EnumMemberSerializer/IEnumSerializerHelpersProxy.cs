using System;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal interface IEnumSerializerHelpersProxy
    {
        void ConfigEnumSerializers(Type type);
    }
}