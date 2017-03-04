using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal static class PrettyEnumHelpers<TEnum> where TEnum : struct 
    {
        //These have to be separate since multiple string values can deserialize to the same enum
        //i.e. "1", "MyEnum", "My Enum" can all resolve to the same enum value.
        internal static ConcurrentDictionary<TEnum, string> SerializeCache = new ConcurrentDictionary<TEnum, string>();
        internal static ConcurrentDictionary<string, TEnum> DeserializeCache = new ConcurrentDictionary<string, TEnum>();

        public static string GetOptimalEnumDescription(TEnum enumValue) 
        {
            return SerializeEnum(enumValue, SerializeCache);
        }

        public static string GetOptimalEnumDescription(TEnum? enumValue)
        {
            return enumValue.HasValue ? SerializeEnum(enumValue.Value, SerializeCache) : null;
        }

        internal static string SerializeEnum(TEnum enumValue, ConcurrentDictionary<TEnum, string> cache)
        {
            return cache.GetOrAdd(enumValue, SerializeEnum);
        }

        internal static string SerializeEnum(TEnum enumValue)
        {
            if (!typeof (TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException();
            }

            var attribute = GetEnumMemberAttribute(enumValue);
            var attributeValue = attribute == null ? string.Empty : attribute.Value;

            var stringValue = string.IsNullOrWhiteSpace(attributeValue)
                                     ? enumValue.ToString()
                                     : attributeValue;

            return stringValue;
        }

        private static EnumMemberAttribute GetEnumMemberAttribute(object enumVal)
        {
            var memberInfo =
                enumVal.GetType().GetTypeInfo().DeclaredMembers.FirstOrDefault(x => x.Name == enumVal.ToString());
            return memberInfo?.GetCustomAttribute<EnumMemberAttribute>(false);
        }

        public static TEnum GetEnumFrom(string enumValue)
        {
            return DeserializeEnum(enumValue, DeserializeCache);
        }

        public static TEnum? GetNullableEnumFrom(string enumValue)
        {
            return DeserializeNullableEnum(enumValue, DeserializeCache);
        }

        internal static TEnum DeserializeEnum(string enumValue, ConcurrentDictionary<string, TEnum> cache)
        {
            return enumValue == null ? default(TEnum) : cache.GetOrAdd(enumValue, DeserializeEnum);
        }

        internal static TEnum? DeserializeNullableEnum(string enumValue, ConcurrentDictionary<string, TEnum> cache)
        {
            return enumValue == null ? default(TEnum?) : cache.GetOrAdd(enumValue, DeserializeEnum);
        }

        internal static TEnum DeserializeEnum(string enumValue)
        {
            if (!typeof (TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException();
            }

            if (TryGetValueFromDescription(enumValue, out TEnum enumObject))
                return enumObject;

            Enum.TryParse(enumValue, true, out enumObject);
            return enumObject;
        }

        private static bool TryGetValueFromDescription(string description, out TEnum enumObject)
        {
            var type = typeof (TEnum);

            foreach (var field in type.GetTypeInfo().DeclaredFields)
            {
                var attribute = field.GetCustomAttribute<EnumMemberAttribute>();

                if (attribute.MatchesDescription(description) || field.MatchesDescription(description))
                {
                    enumObject = (TEnum)field.GetValue(null);
                    return true;
                }
            }
            enumObject = default(TEnum);
            return false;
        }
    }
}