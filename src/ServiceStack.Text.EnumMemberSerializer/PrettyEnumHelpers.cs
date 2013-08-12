using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace ServiceStack.Text.EnumMemberSerializer
{
    /// <summary>
    ///     Serialize/Deserialize enumerations using EnumMember Attribute Value if present.
    /// </summary>
    /// <typeparam name="TEnum">The type must be an enumeration.</typeparam>
    public static class PrettyEnumHelpers<TEnum> where TEnum : struct
    {
        //These have to be separate since multiple string values can deserialize to the same enum
        //i.e. "1", "MyEnum", "My Enum" can all resolve to the same enum value.
        internal static ConcurrentDictionary<TEnum, string> SerializeCache = new ConcurrentDictionary<TEnum, string>();
        internal static ConcurrentDictionary<string, TEnum> DeserializeCache = new ConcurrentDictionary<string, TEnum>();

        /// <summary>
        ///     Gets the optimal string representation of an enumeration.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns>Returns EnumMember Value if present, otherwise returns enumValue.ToString().</returns>
        /// <exception cref="InvalidOperationException">The provided object is not an enumeration.</exception>
        public static string GetOptimalDescription(TEnum enumValue)
        {
            return SerializeEnum(enumValue, SerializeCache);
        }

        internal static string SerializeEnum(TEnum enumValue, ConcurrentDictionary<TEnum, string> cache)
        {
            return cache.GetOrAdd(enumValue, SerializeEnum);
        }

        internal static string SerializeEnum(TEnum enumValue)
        {
            if (!typeof (TEnum).IsEnum)
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

        /// <summary>
        ///     Gets the enumeration for the given string representation.
        /// </summary>
        /// <param name="enumValue">The pretty value, enumeration string value, or integer value of the enumeration.</param>
        /// <exception cref="InvalidOperationException">The type parameter is not an Enumeration.</exception>
        public static TEnum GetEnumFrom(string enumValue)
        {
            return DeserializeEnum(enumValue, DeserializeCache);
        }

        internal static TEnum DeserializeEnum(string enumValue, ConcurrentDictionary<string, TEnum> cache)
        {
            return enumValue == null ? default(TEnum) : cache.GetOrAdd(enumValue, DeserializeEnum);
        }

        internal static TEnum DeserializeEnum(string enumValue)
        {
            if (!typeof (TEnum).IsEnum)
            {
                throw new InvalidOperationException();
            }

            TEnum enumObject = Enum.TryParse(enumValue, true, out enumObject)
                                   ? enumObject
                                   : GetValueFromDescription(enumValue);

            return enumObject;
        }

        private static TEnum GetValueFromDescription(string description)
        {
            Type type = typeof (TEnum);

            foreach (FieldInfo field in type.GetFields())
            {
                var attribute =
                    Attribute.GetCustomAttribute(field, typeof (EnumMemberAttribute)) as EnumMemberAttribute;

                if (attribute.MatchesDescription(description) || field.MatchesDescription(description))
                {
                    return (TEnum) field.GetValue(null);
                }
            }

            return default(TEnum);
        }
    }
}