using System;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerInitializer<T> where T : struct
    {
        public EnumSerializerInitializer()
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("Type parameter must be an enum.");
            }

            JsConfig<T>.Reset();
            JsConfig<T>.SerializeFn = PrettyEnumHelpers<T>.GetOptimalDescription;
            JsConfig<T>.DeSerializeFn = PrettyEnumHelpers<T>.GetEnumFrom;
        }
    }
}