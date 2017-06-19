using System;
using System.Reflection;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerInitializer<TEnum> where TEnum : struct
    {
        public EnumSerializerInitializer()
        {
            if (!typeof (TEnum).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("Type parameter must be an enum.");
            }
        }
        
        public void InitializeEnumSerializer()
        {
            JsConfig<TEnum>.Reset();
            JsConfig<TEnum>.SerializeFn = PrettyEnumHelpers<TEnum>.GetOptimalEnumDescription;
            JsConfigWrapper<TEnum>.SetDeserializerMember(PrettyEnumHelpers<TEnum>.GetEnumFrom);
            //ServiceStack.Text will never use the serialize / deserialize fn if the value is null 
            //or the text is null or empty.
            JsConfig<TEnum?>.Reset();
            JsConfig<TEnum?>.SerializeFn = PrettyEnumHelpers<TEnum>.GetOptimalEnumDescription;
            JsConfigWrapper<TEnum?>.SetDeserializerMember(PrettyEnumHelpers<TEnum>.GetNullableEnumFrom);
        }
    }
}