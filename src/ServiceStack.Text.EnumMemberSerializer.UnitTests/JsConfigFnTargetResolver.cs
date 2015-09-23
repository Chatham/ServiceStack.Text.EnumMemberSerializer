using System;

namespace ServiceStack.Text.EnumMemberSerializer.UnitTests
{
    internal static class JsConfigFnTargetResolver<T>
    {
        public static object GetDeserializerTarget()
        {
            return GetDeserializerTarget("DeSerializeFn");
        }

        private static object GetDeserializerTarget(string name)
        {
            var field = typeof(JsConfig<T>).GetField(name);
            object value;
            if (field != null)
            {
                value = field.GetValue(null);
            }
            else
            {
                var property = typeof(JsConfig<T>).GetProperty(name);
                value = property.GetValue(null, null);
            }

            return ((Func<string, T>)value).Target;
        }
    }
}