using System;

namespace ServiceStack.Text.EnumMemberSerializer
{
    internal class JsConfigWrapper<T>
    {
        public static void SetDeserializerMember(Func<string, T> deserializeFunc)
        {
            SetDeserializerMemberByName("DeSerializeFn", deserializeFunc);
        }
        
        public static void SetDeserializerMemberByName(string memberName, Func<string, T> deserializeFunc)
        {
            var setDeserializer = GetFieldOrNull(memberName) ?? GetPropertyOrNull(memberName);

            if (setDeserializer == null)
            {
                throw new MemberAccessException(string.Format("Unable to find a field or property member named {0}.",
                    memberName));
            }

            setDeserializer(deserializeFunc);
        }

        private static Action<Func<string, T>> GetFieldOrNull(string fieldName)
        {
            var field = typeof(JsConfig<T>).GetField(fieldName);
            if (field == null)
            {
                return null;
            }
            return x => field.SetValue(null, x);
        }

        private static Action<Func<string, T>> GetPropertyOrNull(string propertyName)
        {
            var property = typeof(JsConfig<T>).GetProperty(propertyName);
            if (property == null)
            {
                return null;
            }
            return x => property.GetSetMethod().Invoke(null, new object[] { x });
        }
    }

}