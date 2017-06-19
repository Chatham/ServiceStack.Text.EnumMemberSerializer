using System;
using System.Reflection;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class JsConfigWrapper<T>
    {
        public static void SetDeserializerMember(Func<string, T> deserializeFunc)
        {
            SetDeserializerMemberByName(nameof(JsConfig<object>.DeSerializeFn), deserializeFunc);
        }
        
        public static void SetDeserializerMemberByName(string memberName, Func<string, T> deserializeFunc)
        {
            var setDeserializer = GetFieldOrNull(memberName) ?? GetPropertyOrNull(memberName);

            if (setDeserializer == null)
            {
                throw new MemberAccessException($"Unable to find a field or property member named {memberName}.");
            }

            setDeserializer(deserializeFunc);
        }

        private static Action<Func<string, T>> GetFieldOrNull(string fieldName)
        {
            var field = typeof(JsConfig<T>).GetRuntimeField(fieldName);
            if (field == null)
            {
                return null;
            }
            return x => field.SetValue(null, x);
        }

        private static Action<Func<string, T>> GetPropertyOrNull(string propertyName)
        {
            var property = typeof(JsConfig<T>).GetRuntimeProperty(propertyName);
            if (property == null)
            {
                return null;
            }
            return x => property.SetMethod.Invoke(null, new object[] { x });
        }
    }

};