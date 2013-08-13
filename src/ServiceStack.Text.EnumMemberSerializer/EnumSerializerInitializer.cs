using System;
using System.Collections.Concurrent;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerInitializer<TEnum> where TEnum : struct
    {
        internal static ConcurrentDictionary<TEnum, string> SerializeCache = new ConcurrentDictionary<TEnum, string>();
        internal static ConcurrentDictionary<string, TEnum> DeserializeCache = new ConcurrentDictionary<string, TEnum>();

        public EnumSerializerInitializer()
        {
            if (!typeof (TEnum).IsEnum)
            {
                throw new ArgumentException("Type parameter must be an enum.");
            }

            JsConfig<TEnum>.Reset();
            JsConfig<TEnum>.SerializeFn = PrettyEnumHelpers<TEnum>.GetOptimalEnumDescription;
            JsConfig<TEnum>.DeSerializeFn = PrettyEnumHelpers<TEnum>.GetEnumFrom;
        }
    }
}