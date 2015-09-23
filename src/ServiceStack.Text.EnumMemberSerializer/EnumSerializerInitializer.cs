using System;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerInitializer<TEnum> where TEnum : struct
    {
        public EnumSerializerInitializer()
        {
            if (!typeof (TEnum).IsEnum)
            {
                throw new ArgumentException("Type parameter must be an enum.");
            }
        }

        public void InitializeEnumSerializer()
        {
            JsConfig<TEnum>.Reset();
            JsConfig<TEnum>.SerializeFn = PrettyEnumHelpers<TEnum>.GetOptimalEnumDescription;
            JsConfigWrapper<TEnum>.SetDeserializerMember(PrettyEnumHelpers<TEnum>.GetEnumFrom);
        }

        public void InitializeEnumAndNullableEnumSerializer()
        {
            InitializeEnumSerializer();
            //ServiceStack.Text will never use the serialize / deserialize fn if the value is null 
            //or the text is null or empty.
            JsConfig<TEnum?>.Reset();
            JsConfig<TEnum?>.SerializeFn = PrettyEnumHelpers<TEnum>.GetOptimalEnumDescription;
            JsConfigWrapper<TEnum?>.SetDeserializerMember(PrettyEnumHelpers<TEnum>.GetNullableEnumFrom);
        }
    }
}