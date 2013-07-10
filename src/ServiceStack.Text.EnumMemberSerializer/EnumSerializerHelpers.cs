using System;
using System.Runtime.Serialization;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class EnumSerializerHelpers<T> where T : struct
    {
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
            if (!typeof (T).IsEnum)
            {
                throw new InvalidOperationException();
            }

            var attribute = GetEnumMemberAttribute(enumValue);
            var attributeValue = attribute == null ? string.Empty : attribute.Value;

            return string.IsNullOrWhiteSpace(attributeValue) ? enumValue.ToString() : attributeValue;
        }

        private static EnumMemberAttribute GetEnumMemberAttribute(object enumVal)
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());

            if (memInfo.Length == 0)
            {
                return null;
            }

            var attributes = memInfo[0].GetCustomAttributes(typeof (EnumMemberAttribute), false);

            return attributes.IsEmpty() ? null : (EnumMemberAttribute) attributes[0];
        }

        public static T DeserializeEnum(string enumValue)
        {
            if (!typeof (T).IsEnum)
            {
                throw new InvalidOperationException();
            }

            T enumObject;
            return Enum.TryParse(enumValue, true, out enumObject) ? enumObject : GetValueFromDescription(enumValue);
        }

        private static T GetValueFromDescription(string description)
        {
            var type = typeof (T);

            foreach (var field in type.GetFields())
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