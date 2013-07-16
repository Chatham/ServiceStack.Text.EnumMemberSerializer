using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerHelpers<T> where T : struct
    {
        //These have to be separate since multiple string values can deserialize to the same enum
        //i.e. "1", "MyEnum", "My Enum" can all resolve to the same enum value.
        internal static ConcurrentDictionary<T, string> SerializeCache = new ConcurrentDictionary<T, string>();
        internal static ConcurrentDictionary<string, T> DeserializeCache = new ConcurrentDictionary<string, T>();

        public EnumSerializerHelpers()
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("Type parameter must be an enum.");
            }

            JsConfig<T>.Reset();
            JsConfig<T>.SerializeFn = SerializeEnum;
            JsConfig<T>.DeSerializeFn = DeserializeEnum;
        }

        public static string SerializeEnum(T enumValue)
        {
            return SerializeEnum(enumValue, SerializeCache);
        }

        internal static string SerializeEnum(T enumValue, ConcurrentDictionary<T, string> cache)
        {
            return cache.GetOrAdd(enumValue, GetDescriptionFromEnum);
        }

        internal static string GetDescriptionFromEnum(T enumValue)
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException();
            }

            EnumMemberAttribute attribute = GetEnumMemberAttribute(enumValue);
            string attributeValue = attribute == null ? string.Empty : attribute.Value;

            string stringValue = string.IsNullOrWhiteSpace(attributeValue)
                                     ? enumValue.ToString()
                                     : attributeValue;

            return stringValue;
        }

        private static EnumMemberAttribute GetEnumMemberAttribute(object enumVal)
        {
            Type type = enumVal.GetType();
            MemberInfo[] memberInfo = type.GetMember(enumVal.ToString());

            if (memberInfo.Length == 0)
            {
                return null;
            }

            object[] attributes = memberInfo[0].GetCustomAttributes(typeof (EnumMemberAttribute), false);

            return attributes.IsEmpty() ? null : (EnumMemberAttribute) attributes[0];
        }

        public static T DeserializeEnum(string enumValue)
        {
            return DeserializeEnum(enumValue, DeserializeCache);
        }

        internal static T DeserializeEnum(string enumValue, ConcurrentDictionary<string, T> cache)
        {
            return enumValue == null ? default(T) : cache.GetOrAdd(enumValue, GetEnumFromDescription);
        }

        internal static T GetEnumFromDescription(string enumValue)
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException();
            }

            T enumObject = Enum.TryParse(enumValue, true, out enumObject)
                               ? enumObject
                               : GetValueFromDescription(enumValue);

            return enumObject;
        }

        private static T GetValueFromDescription(string description)
        {
            Type type = typeof (T);

            foreach (FieldInfo field in type.GetFields())
            {
                var attribute =
                    Attribute.GetCustomAttribute(field, typeof (EnumMemberAttribute)) as EnumMemberAttribute;

                if (attribute.MatchesDescription(description) || field.MatchesDescription(description))
                {
                    return (T) field.GetValue(null);
                }
            }

            return default(T);
        }
    }
}